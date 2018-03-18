using System.Web.Mvc;
using Newtonsoft.Json;
using testapp.DataModel;
using System;
using System.Net.Mail;
using testapp.Helpers;

namespace testapp.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
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
                    var user = new tblUsers
                    {
                        Email = dto.Email,
                        Surname = dto.Surname,
                        Name = dto.Name,
                        Nik = dto.Nik,
                        City = dto.City,
                        Comment = dto.Comment,
                        Password = dto.Password,
                        TypeId = Int32.Parse(dto.Type)
                    };
                    ctx.tblUsers.Add(user);
                    ctx.SaveChanges();
                    var msg = $"Доброго времени суток, {user.Name}! Вы зарегистрироваль на Феникс 2018!\n " +
                        $"Для активации профиля Вам необходимо сдать взнос организаторам. " +
                        $"Для дальнейшего доступа в личный кабинет используйте Ваш логин ({user.Email}) и пароль ({user.Password}).\n" +
                        $"Письмо сгенерировано автоматически. Отвечать на него не нужно.";
                    NotificationHelper.NotifyByEmail(user.Email, msg);
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
}