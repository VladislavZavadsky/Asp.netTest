using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLogic.Logic;


namespace Task1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string Authorization(string login, string password)
        {
            if (ModelState.IsValid)
            {
                ActionLogic actionLogic = new ActionLogic();
                int gold = actionLogic.UserCheckOut(login, password);
                if (gold != -1)
                {
                    return "U have " + gold + " golds.";
                }
                else
                {
                    return "Incorrect Login or Password.";
                }
                    

                //Data Source=5.39.94.64;Initial Catalog=vlad.z_Task1;Persist Security Info=True;User ID=vlad.z;Password=09Griffon; Min Pool Size=5
            }
            return Json(ModelState).ToString();
        }
    }
}