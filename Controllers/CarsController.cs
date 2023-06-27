using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Car_Showroom_Web_Application.Models;

namespace Car_Showroom_Web_Application.Controllers
{
    public class CarsController : Controller
    {
        private void CheckUser()
        {
            HttpCookie logCookie = Request.Cookies["UserLog"];
            if (logCookie != null)
            {
                User logUser = new User()
                {
                    Id = int.Parse(logCookie.Values["Id"]),
                    Email = logCookie["Email"],
                    UserName = logCookie["UserName"],
                    Password = logCookie["Password"]
                };
                Session["User"] = logUser;
            }
        }

        public ActionResult CarSearch(string term)
        {
            List<string> lsCars = CarDAO.GetCarsByName(term);
            string combinedString = string.Join(",", lsCars.ToArray());
            return Json(lsCars, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCar(string name)
        {
            int c = CarDAO.GetCarId(name);
            return Content("<script>window.location = '/Cars/Detail?id="+c+"';</script>");
        }

        public ActionResult Index()
        {
            CheckUser();

            if (Session["User"] == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "Cars");
            }

            
            List<Car> lsCars = CarDAO.GetCars();

            ViewData["subList"] = lsCars;
            return View();
        }

        public ActionResult Edit(int id)
        {
            CheckUser();
            Car c = CarDAO.getCarById(id);
            ViewData["car"] = c;
            return View();
        }

        public ActionResult Detail(int id)
        {
            CheckUser();
            Car c = CarDAO.getCarById(id);
            ViewData["car"] = c;
            return View();
        }

        public ActionResult Sell(int id)
        {
            CheckUser();
            Car c = CarDAO.getCarById(id);
            ViewData["car"] = c;
            return View();
        }

        public ActionResult Add()
        {
            CheckUser();
            return View();
        }

        public ActionResult Sold()
        {
            CheckUser();
            Dictionary<Car,DateTime> lsCars = CarDAO.GetSoldCars();

            ViewData["soldList"] = lsCars;
            return View();
        }

        public ActionResult Login()
        {
            CheckUser();
            return View();
        }
        
        [HttpPost]
        public ActionResult LoginForm(Models.User user)
        {
            string save = Request.Form["remember"];
            User loginUser = UserDAO.GetUser(user.Email, user.Password);
            if (loginUser == null)
            {
                Session["error"] = "No user found against email";
                return RedirectToAction(actionName: "Login", controllerName: "Cars");
            }
            else if (loginUser.Id == -1)
            {
                Session["error"] = "Invalid Password";
                Session["email"] = user.Email;
                return RedirectToAction(actionName: "Login", controllerName: "Cars");
            }

            Session["User"] = loginUser;
            if (save != null && save.Equals("on"))
            {
                HttpCookie logCookie = new HttpCookie("UserLog");
                logCookie.Values["UserName"] = loginUser.UserName;
                logCookie.Values["PassWord"] = loginUser.Password;
                logCookie.Values["Email"] = loginUser.Email;
                logCookie.Values["Id"] = loginUser.Id + "";

                logCookie.Expires = DateTime.Now.AddDays(30);

                Response.Cookies.Add(logCookie);
            }
            return RedirectToAction(actionName: "Index", controllerName: "Cars");
        }

        public ActionResult Logout()
        {
            HttpCookie logCookie = Request.Cookies["UserLog"];
            if (logCookie != null)
            {
                logCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(logCookie);
            }
            Session.Remove("User");
            return RedirectToAction(actionName: "Login", controllerName: "Cars");
        }

        [HttpPost]
        public ActionResult AddForm(Car obj, string cancel)
        {
            if (string.IsNullOrEmpty(cancel))
            {
                obj.UserId = ((User)Session["User"]).Id;
                int row = CarDAO.InsertCar(obj);
                if (row == 1)
                {
                    Session["success"] = "Car data was added successfully";
                    return RedirectToAction(actionName: "Index", controllerName: "Cars");
                }
                else
                {
                    Session["error"] = "Car data was not added";
                    Session["car"] = obj;
                    return RedirectToAction(actionName: "Add", controllerName: "Cars");
                }
            }
            else
            {
                Session["error"] = "Operation was cancelled";
                return RedirectToAction(actionName: "Index", controllerName: "Cars");
            }
        }

        public ActionResult EditForm(Car obj, string cancel)
        {

            if (string.IsNullOrEmpty(cancel))
            {
                obj.UserId = ((User)Session["User"]).Id;
                int row = CarDAO.UpdateCar(obj);
                if (row == 1)
                {
                    Session["success"] = "Car data was updated successfully";
                    return RedirectToAction(actionName: "Index", controllerName: "Cars");
                }
                else
                {
                    Session["error"] = "Car data was not updated";
                    ViewData["car"] = obj;
                    return RedirectToAction(actionName: "Edit", controllerName: "Cars");
                }
            }
            else
            {
                Session["error"] = "Operation was cancelled";
                return RedirectToAction(actionName: "Index", controllerName: "Cars");
            }
        }

        public ActionResult SellForm(int id, string cancel)
        {
            if (string.IsNullOrEmpty(cancel))
            {
                int row = CarDAO.SellCar(id);
                if (row == 1)
                {
                    Session["success"] = "Car was sold successfully";
                    return RedirectToAction(actionName: "Index", controllerName: "Cars");
                }
                else
                {
                    Session["error"] = "Car could not be sold";
                    ViewData["car"] = CarDAO.getCarById(id);
                    return RedirectToAction(actionName: "Sell", controllerName: "Cars");
                }
            }
            else
            {
                Session["error"] = "Operation was cancelled";
                return RedirectToAction(actionName: "Index", controllerName: "Cars");
            }
        }
    }
}