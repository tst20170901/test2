﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class StoreCardSingleModel
    {
        public string CardNo { get; set; }
        public string CardPwd { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int BusinessID { get; set; }
        public string SMSContent { get; set; }
    }
}