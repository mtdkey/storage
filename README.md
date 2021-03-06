# MTDKey Storage [<img alt="NuGet" src="https://img.shields.io/nuget/v/MtdKey.Storage"/>](https://www.nuget.org/packages/mtdkey.storage/) <img alt="Licence MIT" src="https://img.shields.io/badge/licence-MIT-green"> <img alt="Platform" src="https://img.shields.io/badge/platform-.Net%206.0-blue"> <img alt="Database" src="https://img.shields.io/badge/database-MySql%20|%20MSSQL-blue">

MTDKey Storage is a backend solution designed to speed up the development of web-applications.
Explore this code, you`ll find many useful features such as:
-	Switching between different databases in real time mode using Entity Framework
-	Using multiple database systems in one application (MSSQL and MySQL)
-	Row Level Security based on Access Token 
-	Control Save, Edit, and Delete operations with a User Access Token
-	Using sql script files and raw requests in an Entity Framework application
-	How to store and retrieve indexed text of unlimited size

<p>
  <strong>Main approach is creating database only once.</strong>
</p>

> You can create application forms to enter information in any quantity without changing the structure of the database. Just create a form template and save  it to the database. You can use the types of form fields – Text (unlimited size), Numeric, Boolean, Date time, List and File when one form is linked to another form.

<p align="center">
  <img src="./img/schema.png" />
  </p>

### Database queries

> The RequestProvider class is the primary gateway for database queries. Use the ContextProperty class to specify connection parameters such as access tokens, connection string, etc. 
You can use two options for connecting to the database.
The first way is to directly specify the connection string and database type (MySQL or MSSQL). For example, if you store the connection string for each user.
The second way is to use the **storage.json** file, ⚠️ which should be in the same folder as the project.

```json
{
  "ConnectionStrings": {
    "mssql_ID1": "Server=.\\SQLEXPRESS;Database=name;User ID=sa;Password=pwd;",
    "mysql_ID2": "Server=127.0.0.1;Database=name;User Id=root;Password=pwd;SslMode=none;"
  }
}
```

> This way can be used if you store unique database IDs for each user, for example: ["user1","ID1"]
The storage.json file can store any quantity of connection strings. Each line must be prefixed to indicate that the database type is "mysql_" or "mssql_".

> You can create a flexible application in which the user creates the databases himself. For example, from the user's personal account. At the same time, there is no need to create a complicated infrastructure.

<p>
Use the Request Provider by specifying a connection string  
</p>

```cs
using RequestProvider requestProvider = new(contextProperty =>
{
    contextProperty.DatabaseType = DatabaseType.MSSQL;
    contextProperty.ConnectionString = "connection string to your MSSQL database";
});
```    

<p>
  Use the query provider by providing a unique database name from storage.json file.
  </p>
  
```cs 
var dataBaseID = "ID1";
var contextProperty = ContextConfig.GetConnectionString(dataBaseID);
var databaseType = ContextConfig.GetDatabaseType(dataBaseID);
using RequestProvider requestProvider = new(contextProperty =>
{
    contextProperty.DatabaseType = databaseType;
    contextProperty.ConnectionString = connectionString;
});
```

> Methods of the RequestProvider class have incoming and out coming arguments as of special Pattern collections, such as Bunch Patter, Field Pattern, Node Patten. 

<p>
  Use the Save method to create or update data. If no identifier is specified in the Pattern as an incoming parameter, a new record will be created.
</p>

```cs
var createdForm = await requestProvider.BunchSaveAsync(pattern => {
                pattern.Name = "Form template name";
                pattern.Description = "Form description";
                pattern.ArchiveFlag = false;
            });
//DataSet is List<BunchPattern> because the RequestProvider may return many forms.
var newID = createdForm.DataSet[0].BunchId;
```
<p>
  Use NodeQuery with a filter to find some data. 
</p>

```cs
var receivedNodes = await requestProvider.NodeQueryAsync(filter =>
{
    filter.SearchText = "search word";
    filter.Page = 2;
    filter.PageSize = 50;
});

var firstNode = receivedNodes.DataSet.FirstOrDefault();
var fields = firstNode.Items;
```

<p>ℹ️ Explore the test code for more examples.</p>
⚠️ To run tests create storage.json file in the Tests project.

```json
{
  "ConnectionStrings": {
    "mssql_test": "Server=.\\SQLEXPRESS;Database=storage;User ID=[USER];Password=[PWD];",
    "mysql_test": "Server=127.0.0.1;Database=storage;User Id=[USER];Password=[PWD];SslMode=none;"
  }
}
```
## License    
Copyright (c) – presented by [Oleg Bruev](https://github.com/olegbruev/).  
MTDKey Storage is free and open-source software licensed under the MIT License.



