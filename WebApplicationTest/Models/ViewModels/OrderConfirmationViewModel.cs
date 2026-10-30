namespace WebApplicationTest.Models.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public Transaction Transaction { get; set; }
        public List<TransactionItem> TransactionItems { get; set; } = new();
    }
}
