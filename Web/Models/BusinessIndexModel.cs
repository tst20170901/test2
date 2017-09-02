using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Models
{
    public class BusinessIndexModel
    {
        public decimal Money { get; set; }
        public BD_Business Business { get; set; }
    }
}