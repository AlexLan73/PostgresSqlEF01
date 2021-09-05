using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBPostgresql.Model
{
    public class ClfFind    // : IBaseField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ClfFindId { get; set; }
        public Project Project { get; set; }
        public Car Car {  get; set; }
        public Mem Mem { get; set; }
        public SecondPath SecondPath { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

//        [ForeignKey("TriggerId")]
        public List<long> Triggers { get; set; }

//        [ForeignKey("TriggerId")]
  //      public List<Trigger> Triggers { get; set; }

    }
}
