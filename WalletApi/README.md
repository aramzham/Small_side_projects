# WalletApi :moneybag:

## **Steps to start working with the project**

1. in package-manager-console type **_update-database_** to create an empty database
2. add a user by **POST api/user/add** specifying in body a json with just one property _{"Name":"YourName"}_ :walking:
3. then via this url **POST api/wallet/add** add some wallets by your taste _{"UserId":1,"Currency":"JPY"}_ :yen:, _{"UserId":1,"Currency":"EUR"}_ :euro:, _{"UserId":1,"Currency":"USD"}_ :heavy_dollar_sign:
4. after you have created the user with wallets you can now:
    * **POST api/wallet/topup** top up in a wallet _{"UserId":1,"Currency":"EUR","Amount":20}_ :sunglasses:
    * **POST api/wallet/withdraw** withdraw funds _{"UserId":1,"Currency":"EUR","Amount":10}_ :money_with_wings:
    * **POST api/wallet/transfer** transfer funds _{"UserId":1,"FromCurrency":"EUR","ToCurrency":"JPY","Amount":5}_ :rocket:
    * **GET api/user/balance/{userId}** get balance :notebook_with_decorative_cover:
    
### Keywords
+ ASP.NET Core 3.1
+ Visual Studio 2019
 
