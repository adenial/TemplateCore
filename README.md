## Work with the project
* Visual Studio 2015 or equivalent with update Web Tools (Asp.NET Core 1.0.1).
* Load references (automatically done with visual studio).

## The project
* Based in: [See](https://docs.asp.net/en/latest/intro.html)
* Upgraded from AspNET Core 1.0.0 to AspNET Core 1.0.1
* Layered project, Model, Repository and Services.
* UnitOfWork (repository)
* Generic Repository
* Role based Authorization & Claims-Based Authorization (Administration Menu) [See](https://docs.asp.net/en/latest/security/authorization/index.html)
* For icons I am using Awesomefont [See](http://fontawesome.io/icons/)
* Users: admin and test, password for both of them 1122334455
* All new accounts are created with the default password: 1122334455
* Implemented Globalization and internationalization (only Index and Login page)
* Working in CRUD of users and roles.

## Unit Testing
* For Repository (database), Using InMemory [See](https://docs.efproject.net/en/latest/providers/in-memory/index.html?highlight=testing)
* For Controllers, Unit Moq 4.6.38-alpha
