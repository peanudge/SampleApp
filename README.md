# Dev Plan

1. Add Login API using built-in authentication middleware.  

- (DB, persistance layer) use SQLite or in-memory DB as fast prototype.
- (BE, domain layer) use hexagon-architecture for simple dependency graph. 
- (BE, test) Add unit case about domain logic. 

2. Add logic in client app using RTK(Redux-ToolKit)
- (test) add unit test using jest 

3. Think simple publishing. (MUI or Manual CSS)
- (test) add storybook

4. Deploy using Azure App Service.

# This project have three-layer

- API, as Web Layer receives requests and routes them to a service in the Domain or business layer
- Domain, as Domain Layer
- Infrastructure, as Persistence Layer to query for or modify the current state of our domain entities.