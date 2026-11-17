using System;
using System.Collections.Generic;

namespace InventoryOrderingSystem
{
    // Stores the information of each system user.
    class UserAccount
    {
        public string Username;
        public string Password;
        public string Role;
        public bool IsActive;
    }

    // Stores product information in the inventory.
    class InventoryProduct
    {
        public string ProductName;
        public int Quantity;
        public bool IsActive;
        public DateTime DateCreated;
        public string CreatedBy;
    }

    // Stores each order and the user who placed it.
    class CustomerOrder
    {
        public string ProductName;
        public int QuantityOrdered;
        public string OrderedBy;
    }

    class Program
    {
        static List<UserAccount> userAccounts = new List<UserAccount>();
        static List<InventoryProduct> inventoryProducts = new List<InventoryProduct>();
        static List<CustomerOrder> customerOrders = new List<CustomerOrder>();

        static bool exitApplication = false;

        static void Main(string[] args)
        {
            AddDefaultUser("admin", "admin123", "Admin");
            AddDefaultUser("Jayson_warehouse", "1234", "Warehouse");
            AddDefaultUser("Jenny_warehouse", "1234", "Warehouse");
            AddDefaultUser("Jayson_user", "1234", "User");
            AddDefaultUser("Jenny_user", "1234", "User");
            AddDefaultUser("Mary_user", "1234", "User");

            AddDefaultProduct("Bread", 30);
            AddDefaultProduct("Mineral Water", 20);
            AddDefaultProduct("Rebisco Chocolate Flavor", 50);

            while (!exitApplication)
            {
                UserAccount loggedInUser = Login();

                if (exitApplication)
                {
                    break;
                }

                if (loggedInUser == null)
                {
                    continue;
                }

                switch (loggedInUser.Role)
                {
                    case "Admin":
                        AdminMenu(loggedInUser);
                        break;

                    case "Warehouse":
                        WarehouseMenu(loggedInUser);
                        break;

                    case "User":
                        UserMenu(loggedInUser);
                        break;
                }
            }

            Console.WriteLine("Thank you for using the system.");
        }

        // Adds a default account when the program starts.
        static void AddDefaultUser(string username, string password, string role)
        {
            UserAccount newUser = new UserAccount();
            newUser.Username = username;
            newUser.Password = password;
            newUser.Role = role;
            newUser.IsActive = true;

            userAccounts.Add(newUser);
        }

        // Adds starting products to the inventory.
        static void AddDefaultProduct(string productName, int quantity)
        {
            InventoryProduct newProduct = new InventoryProduct();
            newProduct.ProductName = productName;
            newProduct.Quantity = quantity;
            newProduct.IsActive = true;
            newProduct.DateCreated = DateTime.Now;
            newProduct.CreatedBy = "System";

            inventoryProducts.Add(newProduct);
        }

        // Reads a whole number. Returns -1 when the input is invalid.
        static int ReadNumber()
        {
            int number;
            string input = Console.ReadLine();

            if (int.TryParse(input, out number))
            {
                return number;
            }

            return -1;
        }

        // Handles role selection and user login.
        static UserAccount Login()
        {
            Console.WriteLine();
            Console.WriteLine("=== ROLE-BASED INVENTORY AND ORDERING SYSTEM ===");
            Console.WriteLine("Please select a user role to login:");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Warehouse");
            Console.WriteLine("3. User");
            Console.WriteLine("4. Exit Application");
            Console.Write("Enter your choice: ");

            int roleChoice = ReadNumber();
            string selectedRole = "";

            switch (roleChoice)
            {
                case 1:
                    selectedRole = "Admin";
                    break;

                case 2:
                    selectedRole = "Warehouse";
                    break;

                case 3:
                    selectedRole = "User";
                    break;

                case 4:
                    exitApplication = true;
                    return null;

                default:
                    Console.WriteLine("Invalid role.");
                    return null;
            }

            Console.Write("Please enter your username: ");
            string enteredUsername = Console.ReadLine();

            Console.Write("Please enter your password: ");
            string enteredPassword = Console.ReadLine();

            foreach (UserAccount account in userAccounts)
            {
                if (account.Username.ToLower() == enteredUsername.ToLower()
                    && account.Password == enteredPassword
                    && account.Role == selectedRole)
                {
                    if (!account.IsActive)
                    {
                        Console.WriteLine("This account is inactive.");
                        return null;
                    }

                    Console.WriteLine("Login successful. Welcome, " + account.Username + "!");
                    return account;
                }
            }

            Console.WriteLine("Wrong username or password.");
            return null;
        }

        // Displays the administrator menu and handles the selected option.
        static void AdminMenu(UserAccount administrator)
        {
            bool isMenuRunning = true;

            while (isMenuRunning)
            {
                Console.WriteLine();
                Console.WriteLine("=== ADMIN MENU ===");
                Console.WriteLine("1. Update Password");
                Console.WriteLine("2. Inactivate User");
                Console.WriteLine("3. Add User");
                Console.WriteLine("4. Display All Users");
                Console.WriteLine("5. Display Inactive Users");
                Console.WriteLine("6. Display Active Users");
                Console.WriteLine("7. Display Products");
                Console.WriteLine("8. Display Orders");
                Console.WriteLine("9. Logout");
                Console.Write("Please select an option: ");

                int menuChoice = ReadNumber();

                switch (menuChoice)
                {
                    case 1:
                        UpdatePassword(administrator);
                        break;

                    case 2:
                        InactivateUser(administrator);
                        break;

                    case 3:
                        AddUser();
                        break;

                    case 4:
                        DisplayUsers("all");
                        break;

                    case 5:
                        DisplayUsers("inactive");
                        break;

                    case 6:
                        DisplayUsers("active");
                        break;

                    case 7:
                        DisplayProducts();
                        break;

                    case 8:
                        DisplayAllOrders();
                        break;

                    case 9:
                        Console.WriteLine("Logged out.");
                        isMenuRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // Allows the administrator to update their own password
        // or the password of another active user.
        static void UpdatePassword(UserAccount administrator)
        {
            List<UserAccount> activeUsers = new List<UserAccount>();

            foreach (UserAccount account in userAccounts)
            {
                if (account.IsActive && account != administrator)
                {
                    activeUsers.Add(account);
                }
            }

            Console.WriteLine();
            Console.WriteLine("--- Update Password ---");
            Console.WriteLine("1. My account");

            for (int i = 0; i < activeUsers.Count; i++)
            {
                Console.WriteLine((i + 2) + ". " + activeUsers[i].Username);
            }

            Console.Write("Select the account: ");
            int accountChoice = ReadNumber();

            UserAccount selectedAccount = null;

            if (accountChoice == 1)
            {
                selectedAccount = administrator;
            }
            else if (accountChoice >= 2 && accountChoice <= activeUsers.Count + 1)
            {
                selectedAccount = activeUsers[accountChoice - 2];
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            Console.Write("Please enter the new password for " + selectedAccount.Username + ": ");
            string newPassword = Console.ReadLine();

            Console.Write("Please confirm the password: ");
            string confirmedPassword = Console.ReadLine();

            if (newPassword == confirmedPassword && newPassword != "")
            {
                selectedAccount.Password = newPassword;
                Console.WriteLine("Password updated successfully.");
            }
            else
            {
                Console.WriteLine("Passwords do not match or the password is empty. Password not changed.");
            }
        }

        // Inactivates an active account, except the administrator's own account.
        static void InactivateUser(UserAccount administrator)
        {
            List<UserAccount> activeUsers = new List<UserAccount>();

            foreach (UserAccount account in userAccounts)
            {
                if (account.IsActive && account != administrator)
                {
                    activeUsers.Add(account);
                }
            }

            if (activeUsers.Count == 0)
            {
                Console.WriteLine("No active users to inactivate.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("--- Inactivate User ---");

            for (int i = 0; i < activeUsers.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + activeUsers[i].Username);
            }

            Console.Write("Please enter the number of the user to inactivate: ");
            int accountChoice = ReadNumber();

            if (accountChoice >= 1 && accountChoice <= activeUsers.Count)
            {
                activeUsers[accountChoice - 1].IsActive = false;
                Console.WriteLine(activeUsers[accountChoice - 1].Username + " has been inactivated.");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        // Creates a new user account after checking the username and role.
        static void AddUser()
        {
            Console.WriteLine();
            Console.WriteLine("--- Add User ---");
            Console.Write("Please enter the new username: ");
            string newUsername = Console.ReadLine();

            if (newUsername == "")
            {
                Console.WriteLine("Username cannot be empty.");
                return;
            }

            foreach (UserAccount account in userAccounts)
            {
                if (account.Username.ToLower() == newUsername.ToLower())
                {
                    Console.WriteLine("That username already exists.");
                    return;
                }
            }

            Console.Write("Please enter the password: ");
            string newPassword = Console.ReadLine();

            if (newPassword == "")
            {
                Console.WriteLine("Password cannot be empty.");
                return;
            }

            Console.WriteLine("Please select the role:");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Warehouse");
            Console.WriteLine("3. User");
            Console.Write("Enter your choice: ");

            int roleChoice = ReadNumber();
            string newRole = "";

            switch (roleChoice)
            {
                case 1:
                    newRole = "Admin";
                    break;

                case 2:
                    newRole = "Warehouse";
                    break;

                case 3:
                    newRole = "User";
                    break;

                default:
                    Console.WriteLine("Invalid role.");
                    return;
            }

            AddDefaultUser(newUsername, newPassword, newRole);
            Console.WriteLine("User has been successfully added.");
        }

        // Displays all users or filters them by active status.
        static void DisplayUsers(string displayType)
        {
            int displayedCount = 0;

            Console.WriteLine();
            Console.WriteLine("--- " + displayType.ToUpper() + " USERS ---");

            foreach (UserAccount account in userAccounts)
            {
                bool shouldDisplay = false;

                switch (displayType)
                {
                    case "all":
                        shouldDisplay = true;
                        break;

                    case "active":
                        if (account.IsActive)
                        {
                            shouldDisplay = true;
                        }
                        break;

                    case "inactive":
                        if (!account.IsActive)
                        {
                            shouldDisplay = true;
                        }
                        break;
                }

                if (shouldDisplay)
                {
                    displayedCount++;

                    string accountStatus = "Inactive";

                    if (account.IsActive)
                    {
                        accountStatus = "Active";
                    }

                    Console.WriteLine();
                    Console.WriteLine("Username: " + account.Username);
                    Console.WriteLine("Role: " + account.Role);
                    Console.WriteLine("Status: " + accountStatus);
                }
            }

            if (displayedCount == 0)
            {
                Console.WriteLine("No users found.");
            }
        }

        // Displays the warehouse menu and handles the selected option.
        static void WarehouseMenu(UserAccount warehouseEmployee)
        {
            bool isMenuRunning = true;

            while (isMenuRunning)
            {
                Console.WriteLine();
                Console.WriteLine("=== WAREHOUSE MENU ===");
                Console.WriteLine("1. Add Products");
                Console.WriteLine("2. Display Products");
                Console.WriteLine("3. Logout");
                Console.Write("Please select an option: ");

                int menuChoice = ReadNumber();

                switch (menuChoice)
                {
                    case 1:
                        AddProducts(warehouseEmployee);
                        break;

                    case 2:
                        DisplayProducts();
                        break;

                    case 3:
                        Console.WriteLine("Logged out.");
                        isMenuRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // Adds one or more products to the inventory.
        static void AddProducts(UserAccount warehouseEmployee)
        {
            Console.WriteLine();
            Console.Write("--- Add Products ---");
            Console.WriteLine();
            Console.Write("Please enter the number of products you want to add: ");

            int numberOfProducts = ReadNumber();

            if (numberOfProducts < 1)
            {
                Console.WriteLine("Invalid number. Please enter at least one product.");
                return;
            }

            for (int i = 1; i <= numberOfProducts; i++)
            {
                InventoryProduct newProduct = new InventoryProduct();

                Console.Write(i + ". Please enter the name of the product: ");
                newProduct.ProductName = Console.ReadLine();

                if (newProduct.ProductName == "")
                {
                    Console.WriteLine("Product name cannot be empty. Product not added.");
                    i--;
                    continue;
                }

                int productQuantity = -1;

                while (productQuantity < 0)
                {
                    Console.Write("Please enter the quantity of the product: ");
                    productQuantity = ReadNumber();

                    if (productQuantity < 0)
                    {
                        Console.WriteLine("Invalid quantity. Please enter zero or a positive whole number.");
                    }
                }

                newProduct.Quantity = productQuantity;
                newProduct.IsActive = true;
                newProduct.DateCreated = DateTime.Now;
                newProduct.CreatedBy = warehouseEmployee.Username;

                inventoryProducts.Add(newProduct);
            }

            Console.WriteLine("All products have been successfully created.");
        }

        // Displays all products and their inventory information.
        static void DisplayProducts()
        {
            Console.WriteLine();
            Console.WriteLine("--- Product List ---");

            if (inventoryProducts.Count == 0)
            {
                Console.WriteLine("No products yet.");
                return;
            }

            for (int i = 0; i < inventoryProducts.Count; i++)
            {
                InventoryProduct product = inventoryProducts[i];

                string activeStatus = "No";

                if (product.IsActive)
                {
                    activeStatus = "Yes";
                }

                Console.WriteLine();
                Console.WriteLine((i + 1) + ". Product Name: " + product.ProductName);
                Console.WriteLine("Quantity: " + product.Quantity);
                Console.WriteLine("Active: " + activeStatus);
                Console.WriteLine("Created Date: " + product.DateCreated);
                Console.WriteLine("Created By: " + product.CreatedBy);
            }
        }

        // Displays the menu for regular users.
        static void UserMenu(UserAccount loggedInUser)
        {
            bool isMenuRunning = true;

            while (isMenuRunning)
            {
                Console.WriteLine();
                Console.WriteLine("=== USER MENU ===");
                Console.WriteLine("1. Order");
                Console.WriteLine("2. Display Order");
                Console.WriteLine("3. Logout");
                Console.Write("Please select an option: ");

                int menuChoice = ReadNumber();

                switch (menuChoice)
                {
                    case 1:
                        MakeOrder(loggedInUser);
                        break;

                    case 2:
                        DisplayMyOrders(loggedInUser);
                        break;

                    case 3:
                        Console.WriteLine("Logged out.");
                        isMenuRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // Allows a user to select a product and place an order.
        static void MakeOrder(UserAccount loggedInUser)
        {
            if (inventoryProducts.Count == 0)
            {
                Console.WriteLine("There are no products to order yet.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("--- Create Order ---");

            for (int i = 0; i < inventoryProducts.Count; i++)
            {
                Console.WriteLine((i + 1) + ". Product Name: " + inventoryProducts[i].ProductName);
                Console.WriteLine("Quantity: " + inventoryProducts[i].Quantity);
                Console.WriteLine();
            }

            Console.Write("Please enter the number of the product you want to order: ");
            int productChoice = ReadNumber();

            if (productChoice < 1 || productChoice > inventoryProducts.Count)
            {
                Console.WriteLine("Invalid product number.");
                return;
            }

            InventoryProduct selectedProduct = inventoryProducts[productChoice - 1];

            if (!selectedProduct.IsActive)
            {
                Console.WriteLine("This product is currently inactive.");
                return;
            }

            if (selectedProduct.Quantity <= 0)
            {
                Console.WriteLine("This product is out of stock.");
                return;
            }

            int orderQuantity = 0;

            while (orderQuantity < 1 || orderQuantity > selectedProduct.Quantity)
            {
                Console.Write("Please enter the quantity you want to order (Available: "
                    + selectedProduct.Quantity + "): ");

                orderQuantity = ReadNumber();

                if (orderQuantity < 1 || orderQuantity > selectedProduct.Quantity)
                {
                    Console.WriteLine("Invalid quantity. Please enter a quantity within the available stock.");
                }
            }

            CustomerOrder newOrder = new CustomerOrder();
            newOrder.ProductName = selectedProduct.ProductName;
            newOrder.QuantityOrdered = orderQuantity;
            newOrder.OrderedBy = loggedInUser.Username;

            customerOrders.Add(newOrder);

            selectedProduct.Quantity = selectedProduct.Quantity - orderQuantity;

            Console.WriteLine();
            Console.WriteLine("Order has been successfully created.");
            Console.WriteLine("Order Details:");
            Console.WriteLine("Product Name: " + newOrder.ProductName);
            Console.WriteLine("Quantity: " + newOrder.QuantityOrdered);
        }

        // Displays only the orders made by the currently logged-in user.
        static void DisplayMyOrders(UserAccount loggedInUser)
        {
            int displayedCount = 0;

            Console.WriteLine();
            Console.WriteLine("--- Your Order History ---");

            foreach (CustomerOrder order in customerOrders)
            {
                if (order.OrderedBy == loggedInUser.Username)
                {
                    displayedCount++;

                    Console.WriteLine();
                    Console.WriteLine("Product Name: " + order.ProductName);
                    Console.WriteLine("Quantity: " + order.QuantityOrdered);
                }
            }

            if (displayedCount == 0)
            {
                Console.WriteLine("You have no orders yet.");
            }
        }

        // Displays every order for the administrator.
        static void DisplayAllOrders()
        {
            Console.WriteLine();
            Console.WriteLine("--- All Orders ---");

            if (customerOrders.Count == 0)
            {
                Console.WriteLine("No orders yet.");
                return;
            }

            foreach (CustomerOrder order in customerOrders)
            {
                Console.WriteLine();
                Console.WriteLine("Product Name: " + order.ProductName);
                Console.WriteLine("Quantity: " + order.QuantityOrdered);
                Console.WriteLine("Ordered By: " + order.OrderedBy);
            }
        }
    }
}
