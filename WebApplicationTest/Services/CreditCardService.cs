using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;

namespace WebApplicationTest.Services
{
    public class CreditCardService
    {
        private readonly CreditCardRepository _creditCardRepos;

        public CreditCardService(CreditCardRepository creditCardRepos)
        {
            _creditCardRepos = creditCardRepos;
        }

        public bool Create(CreditCard card)
        {
            bool createdCard = _creditCardRepos.Create(card);
            return createdCard;
        }

        public bool Update(CreditCard card)
        {
            bool updatedCard = _creditCardRepos.Update(card);
            return updatedCard;
        }

        public CreditCard GetCreditCardByID(int cardID)
        {
            CreditCard card = _creditCardRepos.GetCreditCardByID(cardID);
            return card;
        }

        public IEnumerable<CreditCard> GetSpecificUserCards(int userID)
        {
            var userCards = _creditCardRepos.GetSpecificUserCards(userID);
            return userCards;
        }

        public bool Delete(int cardID)
        {
            var deletedCard = _creditCardRepos.Delete(cardID);
            return deletedCard;
        }
    }
}
