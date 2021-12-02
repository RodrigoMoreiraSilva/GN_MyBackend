using GestaoUnica_backend.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Models
{
    [Table("Demotech_Servicos")]
    public class Demotech_Servico : BaseFileDetail
    {
        [Required]
        [MaxLength(200)]
        [Column("TipoServico")]
        public string TipoServico { get; set; }

        [Required]
        [MaxLength(300)]
        [Column("Categoria")]
        public string Categoria { get; set; }

        [Column("Descricao")]
        public string Descricao { get; set; }
    }
}
