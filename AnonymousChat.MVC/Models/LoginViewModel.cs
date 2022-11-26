using System.ComponentModel.DataAnnotations;

namespace AnonymousChat.MVC.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "You haven't entered your name")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only alphabets and numbers allowed.")]
    public string Name { get; set; }
}