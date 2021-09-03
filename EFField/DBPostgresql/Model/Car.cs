using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBPostgresql.Model
{
    public class Car : IBaseField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CarId { get; set; }

        [MaxLength(15)]
        [Required]
        public string Name { get; set; }
    }
}
