using GestaoUnica_backend.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Services.Models
{
    [Table("Logs")]
    public class Log : BaseEntity
    {
        [Column("Url")]
        public string Url { get; set; }

        public Role Regra { get; set; }

        [MaxLength(100)]
        [Column("AcaoRealizada")]
        public string AcaoRealizada { get; set; }
    }
}
