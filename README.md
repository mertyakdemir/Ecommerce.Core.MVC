### Please change the following fields for the project to work local environment.

#### Ecommerce.UI -> Startup

    services.AddDbContext<IAppContext>(options => options.UseSqlServer
    ("Server=TEST\\SQLEXPRESS;Database=TestDatabase; Integrated Security=SSPI; MultipleActiveResultSets=True;"));

#### Ecommerce.DataLayer -> DatabaseContext

    optionsBuilder.UseSqlServer(
    "Server=TEST\\SQLEXPRESS;Database=TestDatabase; Integrated Security=SSPI; MultipleActiveResultSets=True;");

#### Ecommerce.UI -> appsettings.json
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSSL": true,
    "Email": "your-email-address@gmail.com",
    "Password": "your-password"
    
#### Payment API
#### After completing the order, you can check the order from the page below.

     Card number: 5528790000000008
     Month/Year: 12/2030
     Cvc: 123
     
     https://sandbox-merchant.iyzipay.com/auth/login/expire
     rbf20374@bcaoo.com
     741852
