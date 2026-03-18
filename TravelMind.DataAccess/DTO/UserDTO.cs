namespace TravelMind.DataAccess.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PreferredLanguage { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserDTO(int id, string name, string email, string passwordHash,
            string preferredLanguage, string profileImage, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            PreferredLanguage = preferredLanguage;
            ProfileImage = profileImage;
            CreatedAt = createdAt;
        }
    }
}