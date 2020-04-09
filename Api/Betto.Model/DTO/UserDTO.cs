using Betto.Model.Entities;

namespace Betto.Model.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string MailAddress { get; set; }

        public static explicit operator UserDTO(UserEntity user) => user == null
            ? null
            : new UserDTO
            {
                UserId = user.UserId, 
                Username = user.Username, 
                MailAddress = user.MailAddress
            };
    }
}
