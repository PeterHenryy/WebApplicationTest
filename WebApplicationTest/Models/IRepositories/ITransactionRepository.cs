namespace WebApplicationTest.Models.IRepositories
{
    public interface ITransactionRepository
    {
        bool Create(Transaction transaction);
        IEnumerable<Transaction> GetAllTransactions();
        Transaction GetTransactionByID(int transactionID);
    }
}
