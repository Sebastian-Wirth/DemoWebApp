namespace DemoWebApp.Models
{
    public class MixedViewModel
    {

        public IEnumerable<DemoWebApp.Models.Product> Products { get; set; }

        public IEnumerable<DemoWebApp.Models.Customer> Customers { get; set; }
    }
}
