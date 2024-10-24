using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    public class TicketController : ControllerBase<TicketController>
    {
        public TicketController(IHttpContextAccessor httpContextAccessor, 
                                ILoggerFactory loggerFactory, 
                                IConfiguration configuration, 
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
