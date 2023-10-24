using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SnusProjekat
{
    public class DO
    {
        [Key]
        public string tagName { get; set; }
        public string description { get; set; }
        public string IOaddress { get; set; }
        public string initialValue { get; set; }

        //DO(string tag,string des, string io, string init)
        //{
        //    tagName = tag;
        //    description = des;
        //    IOaddress = io;
        //    initialValue = init;
        //}

        int digitalValue(double value)
        {
            if (value >= 0.5)
                return 1;
            else
                return 0;
        }

    }
}