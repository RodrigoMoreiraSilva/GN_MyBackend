using GestaoUnica_backend.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Services.Models
{
    [Table("DominiosActiveDirectory")]
    public class ActiveDirectoryDomain : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [Column("LdapValue")]
        public string LdapValue { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Nome")]
        public string Name { get; set; }
    }
}
