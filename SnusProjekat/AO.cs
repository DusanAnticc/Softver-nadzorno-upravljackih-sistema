using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SnusProjekat
{
    public class AO
    {
        [Key]
        public string tagName { get; set; }
        public string description { get; set; }
        public string IOaddress { get; set; }
        public double lowLimit { get; set; }
        public double highLimit { get; set; }
        public string initialValue { get; set; }

        //AO(string tag,string des,string io,string low, string high,string init)
        //{
        //    tagName = tag;
        //    description = des;
        //    IOaddress = io;
        //    lowLimit = low;
        //    highLimit = high;
        //    initialValue = init;
        //}

        double checkLimit(double value)
        {
            if (value > highLimit)
                return highLimit;
            else if (value < lowLimit)
                return lowLimit;
            else
                return value;
        }
    }
}