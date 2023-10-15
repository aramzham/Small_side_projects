namespace WalletTask.WebApi.Models.RequestModels.WalletRequestModels
{
    public class BaseWalletRequestModel
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}