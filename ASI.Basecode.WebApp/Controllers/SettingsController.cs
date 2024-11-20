using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ASI.Basecode.WebApp.Controllers
{
    public class SettingsController : ControllerBase<SettingsController>
    {
        public SettingsController(IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveTheme([FromBody] ThemePreference themePreference)
        {
            if (!string.IsNullOrEmpty(themePreference.Theme))
            {
                _httpContextAccessor.HttpContext.Session.SetString("Theme", themePreference.Theme);
            }
            return Ok();
        }
    }

    public class ThemePreference
    {
        public string Theme { get; set; }
    }
}
