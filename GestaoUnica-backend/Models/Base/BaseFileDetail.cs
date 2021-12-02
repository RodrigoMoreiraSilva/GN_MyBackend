using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Models.Base
{
    public class BaseFileDetail : BaseEntity
    {
        [MaxLength(200)]
        [Column("DocumentName")]
        public string DocumentName { get; set; }

        [MaxLength(50)]
        [Column("DocType")]
        public string DocType { get; set; }

        [Column("DocUrl")]
        public string DocUrl { get; set; }
    }
}
