using System.Security.Claims;
using AnonymousChat.Application.CQs.Message.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousChat.MVC.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userName = User.FindFirst(ClaimTypes.Name)!.Value;
        var query = new GetListMessagesQuery() { Name = userName };
        var result = await _mediator.Send(query);
        var messages = result.Messages.ToList();
        return View(messages);
    }
}