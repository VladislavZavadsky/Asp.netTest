using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task1.Views.Home.ViewModels
{
    public class Authorization
    {
        [Required(ErrorMessage = "Вы не ввели логин")]
        [DataType(DataType.Text)]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Логин должен быть не мение 5 и не более 16 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Вы не ввели пароль")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Пароль должен быть не мение 5 и не более 16 символов")]
        public string Password { get; set; }
    }
}