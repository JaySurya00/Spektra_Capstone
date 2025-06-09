namespace EventScheduler.Server.DTOs
{
    public class UserSignupDto
    {
        public string Name { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public DateOnly Dob { get; set; } 
        public string Role { get; set; }
    }
}
