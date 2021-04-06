using NUnit.Framework;
using System;
using System.IO;
// Remember: [UnitOfWork_StateUnderTest_ExpectedBehaviour]

namespace SimpleShop.Test
{
public class Tests
{
    private Keyword[] validKeywords;
    private ShopParser parser = new ShopParser();
    private Customer customer;
    private InvoicePosition invoice;

    [SetUp]
    public void Setup()
    {
        validKeywords = new Keyword[]
        {
            new Keyword("ItemNumber"),
            new Keyword("ItemName"),
            new Keyword("CustomerName"),
            new Keyword("CustomerType"),
            new Keyword("AmountOrdered"),
            new Keyword("NetPrice")
        };

        customer = new Customer
        {
            Name = "Jack"
        };

        invoice = new InvoicePosition
        {
            Customer = new Customer(),
            ItemIdentifier = 0,
            ItemName = "SpringRoll",
            Orders = 2,
            SingleUnitPrice = 3.50m
        };
    }
        
    /// <summary>
    /// Check if the Keyword opening is modified with added <Keyword> 
    /// Rating 0
    /// </summary>
    [Test]
    [Category("Keyword")]
    public void Parsing_KeywordStartTag_AddedBraces()
    {
        // Arrange : initialize the test case
        string expected = "<keyword>";

        // Act : invoke the method we want to test
        string actual = new Keyword("keyword").GetStart();

        // Assert : validate the actual output with the expected result
        Assert.AreEqual(expected, actual);
    }
        
    /// <summary>
    /// Check if the Keyword closing is modified with added </Keyword>
    /// Rating 0
    /// </summary>
    [Test]
    [Category("Keyword")]
    public void Parsing_KeywordEndTag_AddedSlashAndBraces()
    {
        // Arrange : initialize the test case
        string expected = "</keyword>";

        // Act : invoke the method we want to test
        string actual = new Keyword("keyword").GetEnd();

        // Assert : validate the actual output with the expected result
        Assert.AreEqual(expected, actual);
    }
        
    /// <summary>
    /// Set the Keywords and check if they are valid.
    /// Rating 1
    /// </summary>
    [Test]
    [Category("ShopParser")]
    public void Parsing_SetKeywords_OrderOfKeywordsIsCorrect()
    {
        parser.SetKeywords(validKeywords);
        for (int i = 0; i < parser.GetKeywords().Length; ++i)
        {
            // Check if the order of keywords is correct
            Assert.AreEqual(parser.GetKeywords()[i], validKeywords[i]);
        }
    }
        
    /// <summary>
    /// Set the Keyword types and check if they are valid.
    /// Rating 0
    /// </summary>
    [Test]
    [Category("ShopParser")]
    public void ShopParser_SetKeyword_Typ()
    {
        
        validKeywords[0] = new Keyword("ItemNumber", KeywordTypes.Int);
        validKeywords[1] = new Keyword("ItemName", KeywordTypes.String);
        validKeywords[2] = new Keyword("CustomerName", KeywordTypes.String);
        validKeywords[3] = new Keyword("CustomerType", KeywordTypes.String);
        validKeywords[4] = new Keyword("AmountOrdered", KeywordTypes.Decimal);
        validKeywords[5] = new Keyword("NetPrice", KeywordTypes.Decimal);

        parser.SetKeywords(validKeywords);

        for (int i = 0; i < parser.GetKeywords().Length; ++i)
        {
            // Check if the type of keywords is correct
            Assert.AreEqual(parser.GetKeywords()[i].WhichType(), validKeywords[i].WhichType());
        }
    }

    /// <summary>
    /// Check if the parser works correctly.
    /// Make examples and see if you can find problems with the code.
    /// Literals represent KeywordPairs with different Keywords
    /// A B C D
    /// Rating 2
    /// </summary>
    [Test]
    [Category("ShopParser")]
    public void Parsing_ValidFindings_True()
    {
        KeywordPair[] pairs = new KeywordPair[4];
        pairs[0] = new KeywordPair(new Keyword("A"), "1");
        pairs[1] = new KeywordPair(new Keyword("B"), "2");
        pairs[2] = new KeywordPair(new Keyword("C"), "3");
        pairs[3] = new KeywordPair(new Keyword("D"), "4");

        // valid keyword set (A,B,C,D)
        Assert.IsTrue(ShopParser.ValidateFindings(pairs));
    }

    /// <summary>
    /// Check if the parser works correctly.
    /// This time you should check if repetition invalidates the findings.
    /// A A B B C C
    /// Rating 2
    /// </summary>
    [Test]
    [Category("ShopParser")]
    public void Parsing_InvalidatedFindingsWithRepeatedEntry_False()
    {
        KeywordPair[] pairs = new KeywordPair[6];
        pairs[0] = new KeywordPair(new Keyword("A"), "1");
        pairs[1] = new KeywordPair(new Keyword("A"), "1");
        pairs[2] = new KeywordPair(new Keyword("B"), "2");
        pairs[3] = new KeywordPair(new Keyword("B"), "2");
        pairs[4] = new KeywordPair(new Keyword("C"), "3");
        pairs[5] = new KeywordPair(new Keyword("C"), "3");

        // invalid keyword set (A,A,B,B,C,C)
        Assert.IsFalse(ShopParser.ValidateFindings(pairs));
    }
        
    /// <summary>
    /// Check if the parser works correctly.
    /// This time with circular keywords.
    /// A B C A
    /// Rating 2
    /// </summary>
    [Test]
    [Category("ShopParser")]
    public void Parsing_InvalidatedFindingsCircular_False()
    {
        KeywordPair[] pairs = new KeywordPair[4];
        pairs[0] = new KeywordPair(new Keyword("A"), "1");
        pairs[1] = new KeywordPair(new Keyword("B"), "2");
        pairs[2] = new KeywordPair(new Keyword("C"), "3");
        pairs[3] = new KeywordPair(new Keyword("A"), "1");

        // invalid keyword set (A,B,C,A)
        Assert.IsFalse(ShopParser.ValidateFindings(pairs));
    }
        
    /// <summary>
    /// See Tagfile (SampleOrder.tag) for more information.
    /// Are the correct number of keywords recognized? 
    /// Rating 1
    /// </summary>
    [Test]
    [Category("ShopParser")]
    public void Parsing_KeywordsSetTagString_CorrectNumberOfEntries()
    {
        parser.SetKeywords(validKeywords);

        string order = new string( "<invalidKeyword1>0</invalidKeyword1>" +
                                   "<invalidKeyword2>0</invalidKeyword2>" +
                                   "<ItemNumber>1</ItemNumber>" +
                                   "<ItemName>Burger</ItemName>" +
                                   "<CustomerName>James T. Kirk</CustomerName>" +
                                   "<AmountOrdered>2</AmountOrdered>" +
                                   "<NetPrice>8.00</NetPrice>" );

        KeywordPair[] findings = ShopParser.ExtractFromTAG(parser, order);

        // Number of valid keywords in each order should not be greater than the number of keywords in the array "validKeywords"
        Assert.That(findings.Length, Is.AtMost(parser.GetKeywords().Length));
    }
        
    /// <summary>
    /// Again consult the Tagfile for more information.
    /// The parsing should follow the order of the keywords you provided.
    /// Make sure to make it adaptable to different configurations.
    /// Rating 2
    /// </summary>
    [Test]
    [Category("ShopParser")]
    public void Parsing_KeywordsSetTagString_ListOfProvidedTagsInOrder()
    {   
        // Swap the first and last element in the array "validkeywords" for different configurations
        var tmp = validKeywords[0];
        validKeywords[0] = validKeywords[5];
        validKeywords[5] = tmp;    

        parser.SetKeywords(validKeywords);

        string order = new string( "<ItemNumber>1</ItemNumber>" +
                                   "<ItemName>Burger</ItemName>" +
                                   "<CustomerName>James T. Kirk</CustomerName>" +
                                   "<AmountOrdered>2</AmountOrdered>" +
                                   "<NetPrice>8.00</NetPrice>" );
        
        KeywordPair[] findings = ShopParser.ExtractFromTAG(parser, order);

        for (int index = 0; index < findings.Length - 1; ++index)
        {
            // The parsing should follow the order of the array "validkeywords"
            Assert.IsTrue( Array.IndexOf(parser.GetKeywords(), findings[index + 1].Key)
                           > Array.IndexOf(parser.GetKeywords(), findings[index].Key) );
        }
    }

    /// <summary>
    /// Test if the VAT is calculated correctly for the Customer.CalculatePrice
    /// Rating 1
    /// </summary>
    [Test]
    [Category("Customer")]
    public void Invoice_CalculateNormalCustomer_AddValueAddedTax()
    {
        decimal basePrice = 100;
        decimal actual = customer.CalculatePrice(basePrice);
        decimal expected = (1 + Customer.ValueAddedTax) * basePrice;
        Assert.AreEqual(expected, actual);
    }
        
    /// <summary>
    /// Test if the function CreateCustomer returns a customer
    /// Rating 0
    /// </summary>
    [Test]
    [Category("Customer")]
    public void Invoice_CreateCustomer_ReturnsCustomer()
    {
        var result = Customer.CreateCustomer("Somebody");
        Assert.That(result, Is.TypeOf<Customer>());
    }
        
    /// <summary>
    /// Test if the InvoicePosition.Price calculates correctly:
    /// Provided Orders, NetPrice is set.
    /// Rating 1
    /// </summary>
    [Test]
    [Category("Invoice")]
    public void Invoice_OrdersAndNetPriceValid_CalculateCorrectPrice()
    {
        var NetPrice = (1 + Customer.ValueAddedTax) * (invoice.SingleUnitPrice * invoice.Orders);
        Assert.AreEqual( invoice.Price(), NetPrice );
    }
}   // class Tests
}   // namespace SimpleShop.Test