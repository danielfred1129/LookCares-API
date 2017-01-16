using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TlcApiService.Models
{
    public class LoginBindingModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }

    public class UserBindingModel
    {
        [Required]
        [Display(Name = "Token")]
        public string Token { get; set; }

        [Required]
        [Display(Name = "UserKey")]
        public string UserKey { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "ClientKey")]
        public string ClientKey { get; set; }

    }
}