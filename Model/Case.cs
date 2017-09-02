using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class Case
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string TitleSpell { get; set; }
        public string TitleWeb { get; set; }
        public string KeyWords { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Body { get; set; }
        public int TypeID { get; set; }
        public int Hit { get; set; }
        public int Hot { get; set; }
        public string Author { get; set; }
        public string Img { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CompanyName { get; set; }
        public DateTime CDate { get; set; }
    }
}

