namespace ASC.Web.Areas.Accounts.Models
{
    public class RegistrationModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public bool IsEdit { get; set; }
    }
}
