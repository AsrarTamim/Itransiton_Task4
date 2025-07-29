using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Taks5.Entities;
using Taks5.Models;
using Taks5.UnitOfWork;

namespace Taks5.Service
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Registration(RegistrationViewModel model)
        {
            try
            {
                var user = _mapper.Map<UserAccount>(model);
                user.Password = model.Password ?? string.Empty;
                user.CreatedAt = DateTime.Now;
                _unitOfWork.User.add(user);
                _unitOfWork.Save(); 
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Users_Email") == true ||
                    ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    throw new ApplicationException("This email is already registered.");
                }

                throw; 
            }
        }
        public UserAccount Login(LoginViewModel model)
        {
            var user = _unitOfWork.User.GetByEmail(model.Email);

            if (user == null)
                throw new ApplicationException("Invalid email or password.");

            if ((user.Password ?? string.Empty).Trim() != (model.Password ?? string.Empty).Trim())
                throw new ApplicationException("Invalid email or password.");

            if (user.IsBlocked)
                throw new ApplicationException("Your account is blocked.");

            user.LastLogin = DateTime.Now;
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();

            return user;
        }
        public List<UserListViewModel> GetAllUsers()
        {
            var users = _unitOfWork.User.GetAll()
                .OrderByDescending(u => u.LastLogin)
                .ToList();

            return _mapper.Map<List<UserListViewModel>>(users);
        }
        public UserAccount GetUserByEmail(string email)
        {
            return _unitOfWork.User.GetByEmail(email);
        }

        public void UpdateUser(UserAccount user)
        {
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
        }

        public void DeleteUser(string email)
        {
            var user = _unitOfWork.User.GetByEmail(email);
            if (user != null)
            {
                _unitOfWork.User.Delete(user);
                _unitOfWork.Save();
            }
        }



    }
}
