﻿using System.ComponentModel.DataAnnotations;

namespace TemplateCore.ViewModels.Account
{
  public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
