using System.Security.Claims;
using AnonymousChat.Application.CQs.Message.Queries;
using AnonymousChat.Application.CQs.User.Queries;
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
    
    [HttpPost]
    public async Task<IActionResult> GetNames()
    {
        var query = new GetListUsersQuery();
        var result = await _mediator.Send(query);
        var names = result.Names.ToList();
        return Ok(names);
    }
}