using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Betto.Model.Entities
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }
        [Required, StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }
        [Required, Column(TypeName = "varchar(100)")]
        public string MailAddress { get; set; }
        [Required, MaxLength(64)]
        public byte[] PasswordHash { get; set; }
        public ICollection<TicketEntity> Tickets { get; set; }
    }
}
