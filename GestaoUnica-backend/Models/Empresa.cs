using GestaoUnica_backend.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Models
{
    [Table("Empresas")]
    public class Empresa : BaseEntity
    {
        [Required]
        [MaxLength(300)]
        [Column("RazaoSocial")]
        public string RazaoSocial { get; set; }

        [Required]
        [MaxLength(300)]
        [Column("NomeFantasia")]
        public string NomeFantasia { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("CNPJ")]
        public string CNPJ { get; set; }

        [MaxLength(20)]
        [Column("IE")]
        public string IE { get; set; }

        [MaxLength(15)]
        [Column("CNAE")]
        public string CNAE { get; set; }

        [MaxLength(250)]
        [Column("DescricaoCNAE")]
        public string DescricaoCNAE { get; set; }

        public GrupoEmpresa GrupoEmpresa { get; set; }
    }
}
