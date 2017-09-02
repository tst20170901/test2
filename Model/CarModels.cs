using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class CarModels
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PinYin { get; set; }
        public int ParentID { get; set; }
        public string Brand { get; set; }
        public string BrandPinYin { get; set; }
    }
}

