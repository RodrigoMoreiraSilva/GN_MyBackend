using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoUnica_backend.Models.Base
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Column("DataInclusao")]
        public DateTime DataInclusao { get; set; }

        [Column("IdUserInclusao")]
        public int IdUserInclusao { get; set; }

        [Column("DataAlteracao")]
        public DateTime DataAlteracao { get; set; }

        [Column("IdUserAlteracao")]
        public int IdUserAlteracao { get; set; }

        [Column("Observacao")]
        public string Observacao { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }
    }
}
