using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;
using WebApplicationTest.Models.ViewModels;

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


        public string? ValidateCreditCard(CreditCard card, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(card.NameOnCard))
                return "Name on card is required.";

            if (card.NameOnCard.Length < 3)
                return "Name on card is too short.";

            if (card.NameOnCard.Any(char.IsDigit))
                return "Name on card cannot contain numbers.";

            if (string.IsNullOrWhiteSpace(card.CardNumber))
                return "Please enter the card number.";

            var number = card.CardNumber.Replace(" ", "");

            if (!number.All(char.IsDigit))
                return "Card number must contain only digits.";

            if (number.Length != 16)
                return "Card number must contain exactly 16 digits.";

            if (card.CVV == 0)
                return "Please enter the CVV.";

            string cvv = card.CVV.ToString();

            if (!cvv.All(char.IsDigit))
                return "CVV must contain only digits.";

            if (cvv.Length != 3)
                return "CVV must contain exactly 3 digits.";

            var currentMonth = new DateTime(
                transactionDate.Year,
                transactionDate.Month,
                1);

            if (card.Expiry < currentMonth)
            {
                return "This credit card has expired.";
            }

            return null;
        }
    
    }
}
