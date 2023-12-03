namespace SmartEnergyHub.DAL.Entities.APIUser
{
    internal class User
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
