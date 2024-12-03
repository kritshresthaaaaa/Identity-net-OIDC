namespace Identity.Dto
{
    public class LoginResponseDto
    {
        public bool IsTwoFAEnabled { get; set; }
        public string? AccessToken { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }

    }
}
