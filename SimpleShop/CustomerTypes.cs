namespace SimpleShop
{
    class Company : Customer
    {
        public override decimal CalculatePrice(decimal basePrice)
        {
            // Companies do not need to pay any VAT
            return basePrice;
        }
    }
    class Student : Customer
    {
        public override decimal CalculatePrice(decimal basePrice)
        {
            // 20% discount for students before VAT   
            return (1 + ValueAddedTax) * (basePrice * (decimal)0.8);
        }
    }
}
