using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Wedding_Planner.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace Wedding_Planner
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
 
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("createUser")]
        public IActionResult CreateUser(MainUser mainUser)
        {
            if(ModelState.IsValid)
            {
                var checkEmailForRegister = dbContext.user.Any(e => e.email == mainUser.email);
                if(checkEmailForRegister == true)
                {
                    ModelState.AddModelError("email","Try another email please!!!!");
                    return View("Index");
                }
                PasswordHasher<MainUser> Hasher = new PasswordHasher<MainUser>();
                mainUser.password = Hasher.HashPassword(mainUser, mainUser.password);
                dbContext.user.Add(mainUser);
                dbContext.SaveChanges();
                int user_id = mainUser.userID;
                HttpContext.Session.SetInt32("userID",user_id);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            var weddingList = dbContext.wedding.Include(u => u.guest);
            ViewBag.userID = (int)HttpContext.Session.GetInt32("userID");
            return View(weddingList);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("loginPage")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            if(ModelState.IsValid)
            {
                var userData = dbContext.user.FirstOrDefault(e => e.email == login.log_email);
                if(userData == null)
                {
                    ModelState.AddModelError("log_email","Invalid email");
                    return View("LoginPage");
                }
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(login, userData.password, login.log_password);
                if(result == 0)
                {
                    ModelState.AddModelError("log_password","Invalid password");
                    return View("LoginPage"); 
                }
                HttpContext.Session.SetInt32("userID",userData.userID);
                return RedirectToAction("Dashboard");
            }
            return View("LoginPage");
        }

        [HttpGet("addWedding")]
        public IActionResult AddWedding()
        {
            return View();
        }

        [HttpPost("createWedding")]
        public IActionResult CreateWedding(Guest guest,Wedding wedding)
        {
            if(ModelState.IsValid)
            {
                var date = wedding.date;
                var datenow= DateTime.Now;
                if(wedding.date == DateTime.Today)
                {
                    ModelState.AddModelError("date","Date must be in Future");
                    return View("AddWedding");
                }
                if(wedding.date < DateTime.Today)
                {
                    ModelState.AddModelError("date","Date must be in Future");
                    return View("AddWedding");  
                }
                wedding.userID = (int)HttpContext.Session.GetInt32("userID");
                dbContext.wedding.Add(wedding);
                dbContext.SaveChanges();

                guest.weddingID = wedding.weddingID;
                guest.userID = (int)HttpContext.Session.GetInt32("userID");
                guest.is_available = 1;
                dbContext.guest.Add(guest);
                dbContext.SaveChanges();
                
                return RedirectToAction("AddWedding");
            }
            return View("AddWedding");
        }

        [HttpGet("guest/{weddingID}/{is_available}")]
        public IActionResult Guest_checkAvailability(Guest guest,int weddingID,int is_available)
        {
            guest.weddingID = weddingID;
            guest.userID = (int)HttpContext.Session.GetInt32("userID");
            guest.is_available = is_available;
            dbContext.guest.Add(guest);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("deleteWediing/{weddingID}")]
        public IActionResult DeleteWedding(int weddingID)
        {
            Wedding deleteWedding = dbContext.wedding.FirstOrDefault(d => d.weddingID == weddingID);
            dbContext.wedding.Remove(deleteWedding);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("weddingInfo/{weddingID}")]
        public IActionResult WeddingInfo(int weddingID)
        {
            var weddingInfo = dbContext.wedding.Include(g => g.guest).ThenInclude(u => u.user).FirstOrDefault(w => w.weddingID == weddingID);
            return View(weddingInfo);
        }
    }
}