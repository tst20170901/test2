using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using System.Net;
using System.IO;
using System.Text;
using Web.Models;
using Newtonsoft.Json;
using AliceDAL;
using System.Collections;
using Models;

namespace Web.Controllers
{
    public class CarModelAPIController : Controller
    {
        public ActionResult CarModel()
        {
            string result = CacheManager.Get(CacheKeys.CARMODELSJSON) as string;
            if (result == null)
            {
                List<CarModels> list = DAL.CarModels.GetList();
                List<CarsBrand> lis = new List<CarsBrand>();

                List<CarModels> temp = list.FindAll(x => x.ParentID == 0);
                foreach (var item in temp)
                {
                    CarsBrand c = new CarsBrand();
                    c.id = item.ID;
                    c.name = item.Name;
                    c.py = item.PinYin;
                    List<Cars> ls = new List<Cars>();
                    foreach (var i in list.FindAll(x => x.ParentID == item.ID))
                    {
                        Cars car = new Cars() { id = i.ID, name = i.Name, py = i.PinYin };
                        ls.Add(car);
                    }
                    c.models = ls;
                    lis.Add(c);
                }
                result = JsonConvert.SerializeObject(lis);
                CacheManager.Insert(CacheKeys.CARMODELSJSON, result);
            }
            return Content(result);
        }
    }
}