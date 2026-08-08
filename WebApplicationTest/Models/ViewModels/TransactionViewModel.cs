namespace WebApplicationTest.Models.ViewModels
{
    public class TransactionViewModel
    {
        public Transaction Transaction { get; set; }
        public double TransactionTax { get; set; }
        public List<CreditCard>? UserCards { get; set; }
        public int? ChosenCardID { get; set; }
        public IEnumerable<CartItem>? CartItems { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public int ItemsBought { get; set; }
        public CreditCard UserNewCard { get; set; }

        public string GetSummaryProductCategory(int? categoryID)
        {
            string categoryName = Categories.SingleOrDefault(x => x.ID == categoryID).Name;
            return categoryName;
        }
    }
}
