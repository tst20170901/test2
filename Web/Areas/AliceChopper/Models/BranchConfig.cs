using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class BranchConfig
    {
        public int BranchState { get; set; }
        public string StateMsg { get; set; }
        public int OrderCount { get; set; }
    }
}