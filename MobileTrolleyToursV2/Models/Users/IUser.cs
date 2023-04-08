namespace MobileTrolleyTours.Models.Users
{
    public interface IUser
    {
        int Role { get; set; }
        string? Password { get; set; }
        string? FirstName { get; set; }
        string? LastName { get; set; }
    }
}