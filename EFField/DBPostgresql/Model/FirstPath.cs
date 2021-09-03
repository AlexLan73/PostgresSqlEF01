using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBPostgresql.Model
{
    public class FirstPath : IBaseField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FirstPathId { get; set; }

        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

    }
}
