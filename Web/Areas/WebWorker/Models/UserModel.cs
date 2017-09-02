using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.WebWorker.Models
{
    public class UserModel
    {
        private string plate;
        public string Plate
        {
            get { return plate ?? ""; }
            set { plate = value; }
        }
        private string username;

        public string UserName
        {
            get { return username ?? ""; }
            set { username = value; }
        }
        private string mobile;
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        private string brandshow;

        public string BrandShow
        {
            get { return brandshow ?? ""; }
            set { brandshow = value; }
        }
        private string color;
        public string Color
        {
            get { return color ?? ""; }
            set { color = value; }
        }
        public decimal Money { get; set; }
        private string orderUserName;

        public string OrderUserName
        {
            get { return orderUserName ?? ""; }
            set { orderUserName = value; }
        }
        private string orderMobile;
        public string OrderMobile
        {
            get { return orderMobile ?? ""; }
            set { orderMobile = value; }
        }
        private string orderBrandShow;
        public string OrderBrandShow
        {
            get { return orderBrandShow ?? ""; }
            set { orderBrandShow = value; }
        }
        private string orderItem;
        public string OrderItem
        {
            get { return orderItem ?? ""; }
            set { orderItem = value; }
        }
        private string orderColor;
        public string OrderColor
        {
            get { return orderColor ?? ""; }
            set { orderColor = value; }
        }
        private string vipCard;
        public string VipCard
        {
            get { return vipCard ?? ""; }
            set { vipCard = value; }
        }
        private string coupons;
        public string Coupons
        {
            get { return coupons ?? ""; }
            set { coupons = value; }
        }
    }
}