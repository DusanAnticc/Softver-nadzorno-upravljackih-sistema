using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SnusProjekat
{
    public class DI
    {
        [Key]
        public string tagName { get; set; }
        public string description { get; set; }
        public string driver { get; set; }
        public string IOaddress { get; set; }
        public int scanTime { get; set; }
        public List<alarm> alarms = new List<alarm>();
        public bool onOffscan { get; set; }

        //DI(string tag,string des, string driv,string io,string scan,string ala,string onoff)
        //{
        //    tagName = tag;
        //    description = des;
        //    driver = driv;
        //    IOaddress = io;
        //    scanTime = scan;
        //    alarms = ala;
        //    onOffscan = onoff;
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