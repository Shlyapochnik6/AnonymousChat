using System.Security.Claims;
using AnonymousChat.Application.CQs.User.Commands;
using AnonymousChat.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousChat.MVC.Controllers;

public class LoginController : Controller
{
    private readonly IMediator _mediator;

    public LoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var command = new LoginUserCommand() { Name = model.Name };
        var identity = await _mediator.Send(command);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(identity));
        return RedirectToAction("Index", "Chat");
    }
}