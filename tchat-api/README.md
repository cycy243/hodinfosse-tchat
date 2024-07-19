# Getting started

Before doing anything you'll need to add a new appsettings to the Tchat.API project.
This appsettings should contains this:

```sh
  "GoogleOAuthConfiguration": {
    "ClientId": "[GOOGLE_CLIENT_ID]",
    "ClientSecret": "[GOOGLE_CLIENT_SECRET]"
  },
  "MailConfiguration": {
    "Email": "[GOOGLE_EMAIL]",
    "Password": "[GOOGLE_APP_PASSWORD]",
    "Port": "587",
    "SmtpUri": "smtp.gmail.com"
  },
  "ConnectionStrings": {
    "TchatContext": "[YOUR_CONNECTION_STRING]"
  },
  "Pusher": {
    "AppId": "[PUSHER_CLIENT_ID]",
    "AppKey": "[PUSHER_APP_KEY]",
    "AppSecret": "[PUSHER_APP_SECRET]",
    "Cluster": "[PUSHER_APP_CLUSTER]",
    "Encrypted": true
  },
  "JwtConfiguration": {
    "Secret": "[JWT_SECRET]",
    "Issuer": "[JWT_ISSUER]",
    "Audience": "[JWT_AUDIENCE]",
    "ValidityHours": 2
  },
```

You should replace the information between crochet by your owns.