namespace simple_shop.test
{
public class TextSource
{
        public const string WithoutDiscount = 
  @"#########################################
    #					                    #
    #	    Welcome to the SimpleShop	    #
    #					                    #
    #########################################

    Invoices:
    James T. Kirk, Burger, 2, 19.04
    James T. Kirk, Coke, 2, 8.33
    S'chn T'gai Spock, IceCream, 7, 37.49
    ";

        public const string WithDiscount =
  @"#########################################
    #					                    #
    #	    Welcome to the SimpleShop	    #
    #					                    #
    #########################################

    Invoices:
    James T. Kirk, Burger, 2, 19.04
    James T. Kirk, Coke, 2, 8.33
    S'chn T'gai Spock, IceCream, 7, 29.99
    ";
}   // class TextSource
}   // namespace simple_shop.test