using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBPostgresql.Model
{
    public interface IBaseField
    {
        public string Name { get; set; }
    }
    public class Project : IBaseField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProjectId { get; set; }

        [MaxLength(15)]
        [Required]
        public string Name { get; set; }
    }
}
