using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class User
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PreferredLanguage { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserDTO DTO => new UserDTO(
            Id, Name, Email, PasswordHash,
            PreferredLanguage, ProfileImage, CreatedAt
        );

        public User(UserDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            Name = dto.Name;
            Email = dto.Email;
            PasswordHash = dto.PasswordHash;
            PreferredLanguage = dto.PreferredLanguage;
            ProfileImage = dto.ProfileImage;
            CreatedAt = dto.CreatedAt;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = UserData.AddUser(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return UserData.UpdateUser(DTO);
        }

        public static List<UserDTO> GetAll()
        {
            return UserData.GetAllUsers();
        }

        public static User Find(int id)
        {
            UserDTO dto = UserData.GetUserById(id);
            if (dto != null)
                return new User(dto, enMode.Update);
            return null;
        }

        public static User FindByEmail(string email)
        {
            UserDTO dto = UserData.GetUserByEmail(email);
            if (dto != null)
                return new User(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return UserData.DeleteUser(id);
        }

        public static bool UpdatePassword(int userId, string newPasswordHash)
        {
            return UserData.UpdatePassword(userId, newPasswordHash);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew()) { Mode = enMode.Update; return true; }
                    return false;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
    }
}