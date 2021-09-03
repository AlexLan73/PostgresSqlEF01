using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBPostgresql.Model
{
    public class Trigger : IBaseField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TriggerId { get; set; }

        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
    }
}
