using GestaoUnica_backend.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Models
{
    [Table("GrupoEmpresa")]
    public class GrupoEmpresa : BaseEntity
    {
        [Required]
        [MaxLength(300)]
        [Column("Nome")]
        public string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NomeAbreviado")]
        public string NomeAbreviado { get; set; }

    }
}
