This is the project for E-commerce application implementation based on ASP.NET.

User part application capabilities: 
1. Home page: 
![image](https://github.com/user-attachments/assets/9c285849-14a0-4da8-a580-45083064fe8c)
2. Shopping Cart management: 
![image](https://github.com/user-attachments/assets/70c9580d-5d98-40ac-868e-cc8ec8a567ad)
3. Account Management

Admin part application capabilities: 
1. Categories management with FK relation to Products, UI for each of "management" parts is almost identical
2. Products management with the possibility to add images:
![image](https://github.com/user-attachments/assets/bde56f34-0e3b-4b89-a59a-2fbef6b31d1f)
3. Orders management
4. Companies management with the capability to create "Employee", which gives an opportunity for delayed 30-days payment
5. User management

Tech stack is: 
1. Authentication and Authorization using .Net Identity
2. Dependency Injection using default .Net DI containers
3. SQL Database interactions via Entity Framework with Unit of Work pattern
4. Integration tests with NUnit and WebApplication Factory
5. MVC architechture and Stripe integration
6. State management to remain the shopping cart count per user session
7. Facebook authorization capability
