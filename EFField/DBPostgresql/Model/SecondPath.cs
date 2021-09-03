using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBPostgresql.Model
{
    public class SecondPath :  IBaseField// FirstPath
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SecondPathId { get; set; }

        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        
//        public long FirstPathId { get; set; }
        public FirstPath FirstPath { get; set; }
    }
}
