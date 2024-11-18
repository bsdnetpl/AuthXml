namespace AuthXml.DTO
    {
    public class UserDetailsDto
        {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } // Nowe pole dla roli użytkownika
        }
    }
