using GestaoUnica_backend.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Services.Models
{
    [Table("Usuarios")]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [Column("UserName")]
        public string UserName { get; set; }

        [MaxLength(100)]
        [Column("Password")]
        public string Password { get; set; }

        [MaxLength(50)]
        [Column("Role")]
        public string Role { get; set; }

        [Column("PasswordExpired")]
        public bool PasswordExpired { get; set; }

        [Column("ActiveDirectoryAuth")]
        public bool ActiveDirectoryAuth { get; set; }

        [Column("NumTentativasAcesso")]
        public int NumTentativasAcesso { get; set; }

        [NotMapped]
        public string LdapSection { get; set; }
    }
}
