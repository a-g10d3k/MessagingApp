using MessagingApp.Models;
using Microsoft.AspNetCore.Mvc;
using MessagingApp.Data;
using Microsoft.AspNetCore.SignalR;
using MessagingApp.Hubs;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IHubContext<MessageHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IHubContext<MessageHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{controller}/Messages")]
        public IActionResult Messages(UserModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);
            return View(new MessageModel { Sender = model.Username });
        }

        [HttpPost]
        [Route("{controller}/Messages")]
        public async Task<IActionResult> MessagesPost(MessageModel model)
        {
            model.CreatedAt = DateTime.Now;
            if(!ModelState.IsValid) return View("Messages", model);
            if (!_context.Users.Any(u => u.Username == model.Sender))
                _context.Users.Add(new UserModel { Username = model.Sender });
            if (model.Sender != model.Recipent && !_context.Users.Any(u => u.Username == model.Recipent))
                _context.Users.Add(new UserModel { Username = model.Recipent });
            _context.Messages.Add(model);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group(model.Recipent).SendAsync("ReceiveMessage", model);
            _logger.LogInformation($"A message was sent from {model.Sender} to {model.Recipent}.");
            return RedirectToAction("Messages", new UserModel { Username = model.Sender });
        }

        [HttpGet]
        [Route("{controller}/GetMessages/{username}")]
        public async Task<IActionResult> MessagesPost([FromRoute] string username)
        {
            var inbox = await _context.Messages.Where(i => i.Recipent == username).OrderBy(m => m.CreatedAt).Select(m => new
            {
                Id = m.Id,
                Sender = m.Sender,
                Recipent = m.Recipent,
                Title = m.Title,
                Body = m.Body,
                CreatedAt = m.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss")
            }).ToListAsync();
            if (inbox == null) return Ok(new List<MessageModel>());
            return Ok(inbox);
        }

        [HttpGet]
        [Route("{controller}/SearchUsers")]
        public async Task<IActionResult> Search([FromQuery]string term)
        {
            var usernames = await _context.Users.Where(u => u.Username.StartsWith(term)).Select(u => u.Username).ToListAsync();
            return Ok(usernames);
        }
    }
}