namespace WalletTask.WebApi.Models.RequestModels.WalletRequestModels
{
    public class WalletTransferRequestModel : BaseWalletRequestModel
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
    }
}