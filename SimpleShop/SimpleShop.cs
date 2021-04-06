using System;
using System.IO;

namespace SimpleShop
{
public static class SimpleShop
{
    public static string[] ReadFileLineByLine(string path)
    {
        var reader = new System.IO.StreamReader(path);
        var line_counter = 0;
        var needed_space = 0;

        // determine number of lines to create the correct sized of array
        for (var line = ""; line != null; line = reader.ReadLine(), ++line_counter)
        {
            if (line.Length > 0 && line[0] != '#')
            {
                ++needed_space;
            }
        }

        // Set Position to beginning of file
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        reader.DiscardBufferedData();
            
        // Read actual data
        var lines = new string[needed_space];
            
        for (var tag_lines=0; line_counter > 1; --line_counter)
        {
            var tmp = reader.ReadLine();
            if (tmp[0] == '#') { continue; }
            lines[tag_lines++] = tmp;
        }
        return lines;
    }

    static void PrintWelcome()
    {
        var tmp = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write(  "#########################################\n" +
                        "#\t\t\t\t\t#\n" +
                        "#\tWelcome to the SimpleShop\t#\n" +
                        "#\t\t\t\t\t#\n#" +
                        "########################################\n\n"  );
        Console.ForegroundColor = tmp;
    }

    static void PrintInvoice(InvoicePosition ivp)
    {
        Console.WriteLine(String.Join(", ", new string[] {
            ivp.Customer.Name, ivp.ItemName, ivp.Orders.ToString(), ivp.Price().ToString("0.##")
        }));
    }

    public static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("That is not how you use this shop!");
            return 1;
        }

        if (!File.Exists(args[0]))
        {
            Console.WriteLine("Orders not found!");
            return 1;
        }
            
        PrintWelcome();
        var orders = ReadFileLineByLine(args[0]);
        Console.WriteLine("Invoices:");

        // (1) Setup the ShopParser
        ShopParser parser = new ShopParser();
        var validKeywords = new Keyword[]
        {
            new Keyword("ItemNumber"),
            new Keyword("ItemName"),
            new Keyword("CustomerName"),
            new Keyword("CustomerType"),
            new Keyword("AmountOrdered"),
            new Keyword("NetPrice")
        };
        parser.SetKeywords(validKeywords);
        
        foreach (var order in orders)
        {
            // (2) Parse the "orders"
            KeywordPair[] findings = ShopParser.ExtractFromTAG(parser, order);

            // (3) Create invoices from "orders" (which should be in TAG format)
            var invoice = InvoicePosition.CreateFromPairs(findings);

            // (4) Output the sum for each customer, you must use the PrintInvoice function
            PrintInvoice(invoice);
        }

        return 0;
    }
}   // class SimpleShop
}   // namespace SimpleShop
