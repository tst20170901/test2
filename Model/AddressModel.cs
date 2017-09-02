using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class AddressModel
    {
        public string FormatAddress { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string District { get; set; }
        public string AdCode { get; set; }
        public string Lng { get; set; }
        public string Lat { get; set; }
    }
}
