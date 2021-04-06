using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleShop
{
public class InvoicePosition
{
        /*public uint ItemIdentifier = 0;
        public string ItemName = "";
        public uint AmountOrdered = 0;
        public decimal UnitPrice = 0.0m;
        public Customer Customer;*/

    public Customer Customer { get; set; }
    public uint ItemNumber { get; set; }
    public string ItemName { get; set; }
    public uint AmountOrdered { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal totalPrice()
    {
        TotalPrice = Customer.CalculatePrice(UnitPrice * AmountOrdered);
        return TotalPrice;
    }

    private KeywordPair[] RemoveNonLetterCharacters(KeywordPair[] dirtyPairs)
    {
        // Remove all non-letter characters (e.g., "+%&/") and German characters (ä,ö,ü,ß)
        // from the invalid keyword value
        List<KeywordPair> cleanPairs = new List<KeywordPair>();
        for (int i = 0; i < dirtyPairs.Length; ++i)
        {
            var dirty = dirtyPairs[i].Value;
            var clean = Regex.Replace(dirty, "[^A-Za-z0-9.' ]", "");
            cleanPairs.Add( new KeywordPair(dirtyPairs[i].Key, clean) );
        }
        return cleanPairs.ToArray();
    }

    public static InvoicePosition CreateFromPairs(KeywordPair[] pairs)
    {
        InvoicePosition invoice = new InvoicePosition();
        invoice.Customer = new Customer();

        pairs = invoice.RemoveNonLetterCharacters(pairs);

        foreach (var pair in pairs)
        {
            switch (pair.Key.GetString())
            {
                case "ItemNumber":
                    invoice.ItemNumber = uint.Parse(pair.Value);
                    break;
                case "ItemName":
                    invoice.ItemName = pair.Value;
                    break;
                case "CustomerName":
                    invoice.Customer.Name = pair.Value;
                    break;
                case "CustomerType":
                    invoice.Customer = Customer.CreateCustomer(invoice.Customer.Name, pair.Value);
                    break;
                case "AmountOrdered":
                    invoice.AmountOrdered = uint.Parse(pair.Value);
                    break;
                case "UnitPrice":
                    invoice.UnitPrice = decimal.Parse(pair.Value);
                    break;
                default:
                    Console.WriteLine("<unknown keyword>");
                    break;
            }
        }

        return invoice;
    }
}  // class InvoicePosition
}  // namespace SimpleShop