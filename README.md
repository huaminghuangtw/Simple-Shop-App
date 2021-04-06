# Introduction
As a business owner you probably often have to prepare invoices with the help of billing machines.
This simple shop software is developed for generating invoices with tax calculation based on orders in TAG format (See [SampleOrder.tag](./SimpleShop.Test/SampleOrder.tag)).
The tag file contains information regarding ItemNumber, ItemName, CustomerName, CustomerType, AmountOrdered, NetPrice.
To print invoices for , you need a basis for all your pricing calculations. The value added tax (VAT), for example, is applied to all customer types except for companies.
Also, for students, 20% discount will be apllied before VAT.
By running the program, it will output an invoice with a summary for each customer.

# How to use the code?
1. Change directory to SimpleShopProject\SimpleShop\bin\Debug\netcoreapp3.1
2. Run the command: ```SimpleShop SampleOrder.tag```
3. You should see the following output:
  
