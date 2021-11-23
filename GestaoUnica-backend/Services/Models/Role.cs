using GestaoUnica_backend.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Services.Models
{
    [Table("Regras")]
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [Column("Nome")]
        public string Name { get; set; }
    }
}
