namespace WebApplicationTest.Models.ViewModels
{
    public class UserTransactionsViewModel
    {
        public IEnumerable<Transaction> UserTransactions { get; set; }
        public IEnumerable<Refund> UserRefunds { get; set; }
    }
}
