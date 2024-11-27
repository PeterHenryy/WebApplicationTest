namespace WebApplicationTest.Helpers
{
    public class RefundReasons
    {
        public static List<string> Reasons { get; } = new List<string>
        {
            "Defective Product",
            "Wrong Item Shipped",
            "Item Damaged in Transit",
            "Not As Advertised",
            "Ordered By Mistake",
            "Other"
        };
    }
}
