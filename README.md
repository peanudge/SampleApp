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