namespace Taks5.Models
{
    public class UserListViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsBlocked { get; set; }
    }
}
