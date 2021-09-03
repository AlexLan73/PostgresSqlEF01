using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBPostgresql.Model
{
    public class Mem : IBaseField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MemId { get; set; }

        [MaxLength(2)]
        [Required]
        public string Name { get; set; }
    }
}
