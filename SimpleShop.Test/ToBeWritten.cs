using NUnit.Framework;
using System;
using System.IO;

namespace SimpleShop.Test
{
public class ToBeWritten
{
    private InvoicePosition invoicePosition;
    private KeywordPair[] pairs;

    [SetUp]
    public void Setup()
    {
        invoicePosition = new InvoicePosition
        {
            Customer = new Customer(),
            ItemNumber = 0,
            ItemName = "SpringRoll",
            AmountOrdered = 2,
            UnitPrice = 3.50m
        };
            
        pairs = new KeywordPair[]
        {
            new KeywordPair(new Keyword("ItemNumber"), invoicePosition.ItemNumber.ToString()),
            new KeywordPair(new Keyword("ItemName"), invoicePosition.ItemName),
            new KeywordPair(new Keyword("CustomerName"), invoicePosition.Customer.Name),
            new KeywordPair(new Keyword("CustomerType"), invoicePosition.Customer.Type),
            new KeywordPair(new Keyword("AmountOrdered"), invoicePosition.AmountOrdered.ToString()),
            new KeywordPair(new Keyword("UnitPrice"), invoicePosition.UnitPrice.ToString())
        };
    }

    /// <summary>
    /// Convert a ordered input of KeywordPairs to a valid InvoicePosition.
    /// Rating 2
    /// </summary>
    [Test]
    [Category("InvoicePosition")]
    public void Invoice_CreateOrderOrderedInput_Valid()
    {
        var invoice = InvoicePosition.CreateFromPairs(pairs);
        Assert.AreEqual(invoicePosition.ItemNumber, invoice.ItemNumber);
        Assert.AreEqual(invoicePosition.ItemName.GetType(), invoice.ItemName.GetType());
        Assert.AreEqual(invoicePosition.Customer.Name, invoice.Customer.Name);
        Assert.AreEqual(invoicePosition.AmountOrdered, invoice.AmountOrdered);
        Assert.AreEqual(invoicePosition.UnitPrice, invoice.UnitPrice);
    }
        
    /// <summary>
    /// Same as above but now the KeywordPairs are not in order.
    /// Still valid input (only one Keyword per order).
    /// Rating 3
    /// </summary>
    [Test]
    [Category("InvoicePosition")]
    public void Invoice_CreateOrderInput_Valid()
    {
        pairs.Shuffle();
        var invoice = InvoicePosition.CreateFromPairs(pairs);
        Assert.AreEqual(invoicePosition.ItemNumber, invoice.ItemNumber);
        Assert.AreEqual(invoicePosition.ItemName.GetType(), invoice.ItemName.GetType());
        Assert.AreEqual(invoicePosition.Customer.Name, invoice.Customer.Name);
        Assert.AreEqual(invoicePosition.AmountOrdered, invoice.AmountOrdered);
        Assert.AreEqual(invoicePosition.UnitPrice, invoice.UnitPrice);
    }

    /// <summary>
    /// Now extend the code to make sure that you also take wrong serialization into account.
    /// %%&$5 is not valid number.
    /// Rating 2
    /// </summary>
    [Test]
    public void Invoice_CreateOrderOrderedButWrongInput_DefaultValues()
    {
        pairs[4] = new KeywordPair(new Keyword("AmountOrdered"), "+%&/" + invoicePosition.AmountOrdered.ToString());
        pairs[5] = new KeywordPair(new Keyword("UnitPrice"), invoicePosition.UnitPrice.ToString() + "%&öä/");

        var invoice = InvoicePosition.CreateFromPairs(pairs);
        Assert.AreEqual(invoicePosition.ItemNumber, invoice.ItemNumber);
        Assert.AreEqual(invoicePosition.ItemName.GetType(), invoice.ItemName.GetType());
        Assert.AreEqual(invoicePosition.Customer.Name, invoice.Customer.Name);
        Assert.AreEqual(invoicePosition.AmountOrdered, invoice.AmountOrdered);
        Assert.AreEqual(invoicePosition.UnitPrice, invoice.UnitPrice);
    }
        
    /// <summary>
    /// Branch out Customer.CreateCustomer to provide different customer types.
    /// Add Company!
    /// Rating 2
    /// </summary>
    [Test]
    [Category("Customer")]
    public void Invoice_CreateCustomer_Company()
    {
        var company = Customer.CreateCustomer("Starfleet", "Company");
        Assert.AreEqual(company.GetType().ToString(), "SimpleShop.Company");
        // BaseType == Parent
        Assert.AreEqual(company.GetType().BaseType.ToString(), "SimpleShop.Customer");
    }

    /// <summary>
    /// Branch out Customer.CreateCustomer to provide different customer types.
    /// Add Student!
    /// Rating 1
    /// </summary>
    [Test]
    [Category("Customer")]
    public void Invoice_CreateCustomer_Student()
    {
        // The everlasting scholar
        var student = Customer.CreateCustomer("S'chn T'gai Spock", "SimpleShop.Student");
        // BaseType == Parent
        Assert.AreEqual(student.GetType().BaseType.ToString(), "SimpleShop.Customer");
    }

    /// <summary>
    /// Make it so that the companies do not pay any VAT.
    /// Rating 2-3;
    /// </summary>
    [Test]
    [Category("InvoicePosition")]
    public void Invoice_AmountAndCompanyAndPrice_NoVat()
    {
        // Create company if defined else default to <Customer>, function of <> will be covered in lecture 3 or 4
        var company = TestHelpers.CreateClassIfDefinedOrDefault<Customer>("Company", "SimpleShop");
        invoicePosition.Customer = company;
        var price = invoicePosition.totalPrice();
        Assert.AreEqual((double)price, 7.00, 0.01);
    }

    /// <summary>
    /// Include discount for 20% before VAT
    /// Rating 2-3;
    /// </summary>
    [Test]
    [Category("InvoicePosition")]
    public void Invoice_AmountAndStudentAndPrice_TwentyPercentBeforeVat()
    {
        // Create company if defined else default to <Customer>, function of <> will be covered in lecture 3 or 4
        var student = TestHelpers.CreateClassIfDefinedOrDefault<Customer>("Student", "SimpleShop");
        invoicePosition.Customer = student;
        var price = invoicePosition.totalPrice();
        Assert.AreEqual((double) price, 6.66, 0.01);
    }
        
    /// <summary>
    /// Extend InvoicePosition.CreateFromPairs so it will take to correct customer type!
    /// Rating 2;
    /// </summary>
    [Test]
    [Category("InvoicePosition")]
    public void Invoice_CreateOrderInputWithStudentOrCompany_Valid()
    {
        pairs[3] = new KeywordPair(new Keyword("CustomerType"), "Company");
        var invoice = InvoicePosition.CreateFromPairs(pairs);

        // Make sure that invoice has a set customer, otherwise this will be a NullReferenceExeption
        Assert.AreEqual("SimpleShop.Company", invoice.Customer.GetType().ToString());


        pairs[3] = new KeywordPair(new Keyword("CustomerType"), "Student");
            
        // Make sure that invoice has a set customer, otherwise this will be a NullReferenceExeption
        invoice = InvoicePosition.CreateFromPairs(pairs);
        Assert.AreEqual("SimpleShop.Student", invoice.Customer.GetType().ToString());
    }

    /// <summary>
    /// And finally the output. Complete the main function. Use the function PrintInvoice to get the proper format.
    /// This test will be OK even if you were not able to include a working discount. More information attached to the main function.
    /// Modify the TagFile to make Spock a student.
    /// Rating 2-3 
    /// </summary>
    [Test]
    [Category("InvoicePosition")]
    public void FullProgram_OutputCorrect_Valid()
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            SimpleShop.Main( new string[] { "../../../../SampleOrder.tag" } );
                
            var output = sw.ToString();
            var burger = output.IndexOf("James T. Kirk, Not specified, Burger, 2, 8.00, 19.04", StringComparison.Ordinal) >= 0;
            var ice_cream = output.IndexOf("S'chn T'gai Spock, Not specified, IceCream, 7, 4.50, 37.49", StringComparison.Ordinal) >= 0;
            var bread = output.IndexOf("Siemens AG, Company, Bread, 6, 0.99, 5.94", StringComparison.Ordinal) >= 0;
            var chocolate_discount = output.IndexOf("David, Student, Chocolate, 3, 3.50, 10", StringComparison.Ordinal) >= 0;
            var ice_cream_discount = output.IndexOf("David, Student, IceCream, 1, 4.50, 4.28", StringComparison.Ordinal) >= 0;

            Assert.IsTrue(burger && ice_cream && bread && chocolate_discount && ice_cream_discount);
        }
    }
}   // class ToBeWritten
}   // namespace SimpleShop.Test