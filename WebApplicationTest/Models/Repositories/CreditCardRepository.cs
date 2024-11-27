using WebApplicationTest.Data;
using WebApplicationTest.Models.IRepositories;

namespace WebApplicationTest.Models.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ApplicationDbContext _context;

        public CreditCardRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(CreditCard card)
        {
            try
            {
                _context.CreditCards.Add(card);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public bool Delete(int cardID)
        {
            try
            {
                var card = GetCreditCardByID(cardID);
                _context.CreditCards.Remove(card);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public CreditCard GetCreditCardByID(int cardID)
        {
            var creditCard = _context.CreditCards.SingleOrDefault(x => x.ID == cardID);
            return creditCard;
        }

        public IEnumerable<CreditCard> GetSpecificUserCards(int userID)
        {
            var userCards = _context.CreditCards.Where(x => x.UserID == userID).ToList();
            return userCards;
        }

        public bool Update(CreditCard card)
        {
            try
            {
                _context.CreditCards.Update(card);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
    }
}
