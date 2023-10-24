using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SnusProjekat
{
    public class AI
    {
        [Key]
        public string tagName { get; set; }
        public string description { get; set; }
        public string driver { get; set; }
        public string IOaddress { get; set; }
        public int scanTime { get; set; }
        public List<alarm> alarms = new List<alarm>();
        public bool onOffscan { get; set; }
        public double lowLimit { get; set; }
        public double highLimit { get; set; }
        public string units { get; set; }//nije bitno , ne treba
        

        //AI(string tag, string des, string dri, string io, string sca,string ala, string onoff, string low, string high, string uni)
        //{
        //    tagName = tag;
        //    description = des;
        //    driver = dri;
        //    IOaddress = io;
        //    scanTime = sca;
        //    alarms = ala;
        //    onOffscan = onoff;
        //    lowLimit = low;
        //    highLimit = high;
        //    units = uni;
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