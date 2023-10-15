using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WalletTask.BL;
using WalletTask.WebApi.Models.RequestModels;

namespace WalletTask.WebApi.Controllers
{
    public class UserController : BaseController
    {
        public UserController(WalletTaskBL bl, IConfiguration configuration, ILogger<BaseController> logger) : base(bl, configuration, logger)
        {
        }

        [HttpPost("Add")]
        public async Task Add([FromBody]UserRequestModel userRequestModel)
        {
            try
            {
                await _bl.UserBL.Add(userRequestModel.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Add");
                throw;
            }
        }

        [HttpGet("Balance/{userId}")]
        public async Task<Dictionary<string,decimal>> GetBalance(int userId)
        {
            try
            {
                var balance = await _bl.UserBL.GetBalance(userId);
                return balance;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetBalance");
                throw;
            }
        }
    }
}