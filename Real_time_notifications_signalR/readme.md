To generate a jwt token use this command
```shell
dotnet user-jwts create -n "344BF590-0079-490E-8D74-9539787
4E125"
```
where ```344BF590-0079-490E-8D74-9539787
4E125``` is just a random GUID.

Run it on your backend project and it will also add a json to your ```appsettings.json``` file.
```json lines
"Authentication": {
    "Schemes": {
      "Bearer": {
        "ValidAudiences": [
          "https://localhost:7226"
        ],
        "ValidIssuer": "dotnet-user-jwts"
      }
    }
  }
```