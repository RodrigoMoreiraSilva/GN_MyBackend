using GestaoUnica_backend.Models.Base;
using GestaoUnica_backend.Services.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Models
{
    [Table("Pessoas")]
    public class Pessoa : BaseEntity
    {
        [MaxLength(250)]
        [Column("Nome")]
        public string Nome { get; set; }

        [MaxLength(200)]
        [Column("Email")]
        public string Email { get; set; }

        [MaxLength(200)]
        [Column("Area")]
        public string Area { get; set; }

        public User Usuario { get; set; }
    }
}
