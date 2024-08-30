namespace Service.ViewModels.UserVMS
{
    public class UserVM
    {

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
