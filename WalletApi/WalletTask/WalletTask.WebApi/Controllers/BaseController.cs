using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WalletTask.BL;

namespace WalletTask.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;
        protected ILogger<BaseController> _logger;
        protected WalletTaskBL _bl;

        public BaseController(WalletTaskBL bl, IConfiguration configuration, ILogger<BaseController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _bl = bl;
        }
    }
}