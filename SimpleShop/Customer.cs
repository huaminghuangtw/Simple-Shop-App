namespace SimpleShop
{
public class Customer
{
    public const decimal ValueAddedTax = 0.19m;
    public string Name = "";
    public virtual decimal CalculatePrice(decimal basePrice)
    {
        return (1 + ValueAddedTax) * basePrice;
    }

    public static Customer CreateCustomer(string name, string type="")
    {
        Customer customer = new Customer();
        switch (type)
        {
            case "SimpleShop.Company":
            case "Company":
                Customer company = new Company();
                customer = (Company) company;
                break;
            case "SimpleShop.Student":
            case "Student":
                Customer student = new Student();
                customer = (Student) student;
                break;
            default:
                break;
        }
        customer.Name = name;
        return customer;
    }
}   // class Customer
}   // namespace SimpleShop