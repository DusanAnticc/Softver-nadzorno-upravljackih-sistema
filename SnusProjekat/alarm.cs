using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SnusProjekat
{
    
  
    public class alarm
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public int Priority { get; set; }
        public double Value { get; set; }
        public double TagValue { get; set; }
        
        public string TagName { get; set; }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"Alarm[{TagName}]: type-{Type},priority-{Priority},value-{Value},date-{Date}";
        }
    }
}