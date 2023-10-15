using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WalletTask.BL;
using WalletTask.Common.Helpers.CurrencyHelper;
using WalletTask.WebApi.Models.RequestModels.WalletRequestModels;

namespace WalletTask.WebApi.Controllers
{
    public class WalletController : BaseController
    {
        private readonly ICurrencyHelper _currencyHelper;

        public WalletController(WalletTaskBL bl, ICurrencyHelper currencyHelper, IConfiguration configuration, ILogger<BaseController> logger) : base(bl, configuration, logger)
        {
            _currencyHelper = currencyHelper;
        }

        [HttpPost("Add")]
        public async Task Add([FromBody] WalletRequestModel walletRequestModel)
        {
            try
            {
                await _bl.WalletBL.Add(walletRequestModel.UserId, walletRequestModel.Currency);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Add");
                throw;
            }
        }

        [HttpPost("TopUp")]
        public async Task TopUp([FromBody] WalletRequestModel walletRequestModel)
        {
            try
            {
                await _bl.WalletBL.TopUp(walletRequestModel.UserId, walletRequestModel.Currency, walletRequestModel.Amount);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "TopUp");
                throw;
            }
        }

        [HttpPost("Withdraw")]
        public async Task Withdraw([FromBody] WalletRequestModel walletRequestModel)
        {
            try
            {
                await _bl.WalletBL.Withdraw(walletRequestModel.UserId, walletRequestModel.Currency, walletRequestModel.Amount);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Withdraw");
                throw;
            }
        }

        [HttpPost("Transfer")]
        public async Task Transfer([FromBody] WalletTransferRequestModel walletTransferRequestModel)
        {
            try
            {
                await _bl.WalletBL.Transfer(walletTransferRequestModel.UserId, walletTransferRequestModel.FromCurrency, walletTransferRequestModel.ToCurrency, walletTransferRequestModel.Amount, _currencyHelper);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Transfer");
                throw;
            }
        }
    }
}