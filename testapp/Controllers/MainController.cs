using System.Web.Mvc;
using Newtonsoft.Json;
using testapp.DataModel;
using System;
using System.Linq;
using System.Net.Mail;
using testapp.Helpers;
using testapp.Authorization;
using System.Web;
using System.Collections.Generic;

namespace testapp.Controllers
{
    public class MainController : BaseController
    {
        public ActionResult Index()
        {
            var user = Auth.GetCurrentUser(HttpContext);
            if (user != null)
                return RedirectToRoute(new { controller = "Main", action = "UserPage" });

            return View();
        }

        public ActionResult Register()
        {
            var user = Auth.GetCurrentUser(HttpContext);
            if (user != null)
                return RedirectToRoute(new { controller = "Main", action = "UserPage" });
            return View();
        }

        public ActionResult UserPage()
        {
            var user = Auth.GetCurrentUser(HttpContext);
            if (user == null)
                return RedirectToRoute(new { controller = "Main", action = "Index" });
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginDto dto)
        {
            var result = new
            {
                success = true,
                errorMsg = ""
            };
            try
            {
                using (var ctx = new fenixklgEntities())
                {
                    bool isUserExists = ctx.tblUsers.FirstOrDefault(u => u.Email == dto.Email) != null;
                    if (isUserExists)
                    {
                        
                        var user = Auth.Login(dto.Email, dto.Password, HttpContext);
                        result = user != null
                            ? new
                            {
                                success = true,
                                errorMsg = ""
                            }
                            : new
                            {
                                success = false,
                                errorMsg = "Неверный пароль!"
                            };
                    }
                    else
                        result = new
                        {
                            success = false,
                            errorMsg = "Пользователь не найден!"
                        };
                }
            }
            catch(Exception ex)
            {
                var msg = $"На сайте fenixklg.ru произошло исключение.\n Ошибка: {ex.Message}";
                NotificationHelper.NotifyByEmail("s-t-o-r-m@list.ru", msg);
                result = new
                {
                    success = false,
                    errorMsg = ex.Message
                };
            }
            return Json(result);
        }

        public ActionResult LogOut()
        {
            Auth.LogOut(HttpContext);
            return RedirectToRoute(new { controller = "Main", action = "Index" });
        }

        [HttpPost]
        public JsonResult GetUserInfo()
        {
            var user = Auth.GetCurrentUser(HttpContext);
            tblBalance balance = null;
            using (var ctx = new fenixklgEntities())
            {
                balance = ctx.tblBalance.FirstOrDefault(b => b.UserId == user.Id);
            }
            if (balance == null)
                return Json(new {
                    success = false
                });

            return Json(new
            {
                success = true,
                userInfo = new
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Nik = user.Nik,
                    IsActive = balance.IsEnabled,
                    Balance = balance.Balance,
                    IsBank = balance.IsBank,
                    Code = balance.Code,
                }
            });
        }

        [HttpPost]
        public JsonResult GetNews()
        {
            var news = new List<tblNews>();
            using (var ctx = new fenixklgEntities())
            {
                news = ctx.tblNews.ToList();
            }

            return Json(new {
                success = true,
                news = news.Select(n => new {
                    Content = n.Content,
                    Title = n.Title,
                    Date = n.Date.Value.ToShortDateString()
                })
            });
        }

        [HttpPost]
        public JsonResult AddUser(UserDto dto)
        {
            var result = new
            {
                success = true,
                errorMsg = ""
            };

            try
            {
                using (var ctx = new fenixklgEntities())
                {
                    //проверка на существующий
                    bool isUserExists = ctx.tblUsers.FirstOrDefault(u => u.Email == dto.Email) != null;
                    if (!isUserExists)
                    {
                        var user = new tblUsers
                        {
                            Email = dto.Email,
                            Surname = dto.Surname,
                            Name = dto.Name,
                            Nik = dto.Nik,
                            City = dto.City,
                            Comment = dto.Comment,
                            Password = dto.Password,
                            TypeId = Int32.Parse(dto.Type),
                            RegDate = DateTime.Now
                        };
                        ctx.tblUsers.Add(user);
                        ctx.SaveChanges();
                        var msg = $"Доброго времени суток, {user.Name}! Вы зарегистрироваль на Феникс 2018!\n " +
                            $"Для активации профиля Вам необходимо сдать взнос организаторам. " +
                            $"Для дальнейшего доступа в личный кабинет используйте Ваш логин ({user.Email}) и пароль ({user.Password}).\n" +
                            $"Письмо сгенерировано автоматически. Отвечать на него не нужно.";
                        NotificationHelper.NotifyByEmail(user.Email, msg);
                    }
                    else
                        result = new
                        {
                            success = false,
                            errorMsg = "Email уже зарегистрирован!"
                        };
                }
            }
            catch(Exception ex)
            {
                var msg = $"На сайте fenixklg.ru произошло исключение.\n Ошибка: {ex.Message}";
                NotificationHelper.NotifyByEmail("s-t-o-r-m@list.ru", msg);
                result = new
                {
                    success = false,
                    errorMsg = ex.Message
                };
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult SendWish(string wish)
        {
            var user = Auth.GetCurrentUser(HttpContext);
            var msg = $"Пожелание от {user.Email} : {wish}";
            NotificationHelper.NotifyByEmail("s-t-o-r-m@list.ru", msg);
            NotificationHelper.NotifyByEmail("Vertigo-3112@mail.ru", msg);

            return Json(new
            {
                success = true
            });
        }
    }

    public class UserDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("nik")]
        public string Nik { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class LoginDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}