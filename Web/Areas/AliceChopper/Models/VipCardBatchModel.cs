using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class VipCardBatchModel
    {
        public string CardNoPrefix { get; set; }
        public int CardNoStart { get; set; }
        public int CardNoEnd { get; set; }
        public string Title { get; set; }
        public int TypeID { get; set; }
        public int BusinessID { get; set; }
        public string SMSContent { get; set; }
        public DateTime TDate { get; set; }
    }
}