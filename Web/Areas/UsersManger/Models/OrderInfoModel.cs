﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Areas.UsersManger.Models
{
    public class OrderInfoModel
    {
        public List<UserOrderActions> UserActionsList { get; set; }
        public Orders OrderInfo { get; set; }
        public List<Orders_Item> OrderItems { get; set; }
    }
}