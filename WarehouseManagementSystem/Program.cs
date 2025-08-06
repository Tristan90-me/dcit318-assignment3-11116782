using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem
{
    // Marker interface for inventory items
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // Electronic item class
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }

        public override string ToString()
        {
            return $"[Electronic] ID: {Id}, Name: {Name}, Brand: {Brand}, Qty: {Quantity}, Warranty: {WarrantyMonths} months";
        }
    }

    // Grocery item class
    public class GroceryItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }

        public override string ToString()
        {
            return $"[Grocery] ID: {Id}, Name: {Name}, Qty: {Quantity}, Expiry: {ExpiryDate:dd/MM/yyyy}";
        }
    }

    // Custom exceptions
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // Generic inventory repository
    public class InventoryRepository<T> where T : IInventoryItem
    {
        private Dictionary<int, T> _items = new Dictionary<int, T>();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
                throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
            _items[item.Id] = item;
        }

        public T GetItemById(int id)
        {
            if (!_items.ContainsKey(id))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            return _items[id];
        }

        public void RemoveItem(int id)
        {
            if (!_items.ContainsKey(id))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            _items.Remove(id);
        }

        public List<T> GetAllItems()
        {
            return new List<T>(_items.Values);
        }

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (!_items.ContainsKey(id))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            if (newQuantity < 0)
                throw new InvalidQuantityException("Quantity cannot be negative.");
            _items[id].Quantity = newQuantity;
        }
    }

    // Warehouse manager
    public class WareHouseManager
    {
        private InventoryRepository<ElectronicItem> _electronics = new InventoryRepository<ElectronicItem>();
        private InventoryRepository<GroceryItem> _groceries = new InventoryRepository<GroceryItem>();

        public void Run()
        {
            Console.Title = "🏬 Warehouse Inventory Management System";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Welcome to the Warehouse Inventory Management System ===\n");
            Console.ResetColor();

            SeedData();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Electronic Items ---");
            Console.ResetColor();
            PrintAllItems(_electronics);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Grocery Items ---");
            Console.ResetColor();
            PrintAllItems(_groceries);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n--- Running Original Exception Tests ---");
            Console.ResetColor();

            RunOriginalTests();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nOriginal tests complete. Now entering interactive mode...");
            Console.ResetColor();

            InteractiveMenu();
        }

        private void SeedData()
        {
            // Add electronics
            _electronics.AddItem(new ElectronicItem(1, "Smartphone", 50, "Samsung", 12));
            _electronics.AddItem(new ElectronicItem(2, "Laptop", 20, "Dell", 24));
            _electronics.AddItem(new ElectronicItem(3, "Headphones", 100, "Sony", 6));

            // Add groceries
            _groceries.AddItem(new GroceryItem(101, "Apples", 200, DateTime.Now.AddMonths(1)));
            _groceries.AddItem(new GroceryItem(102, "Milk", 50, DateTime.Now.AddDays(7)));
            _groceries.AddItem(new GroceryItem(103, "Bread", 80, DateTime.Now.AddDays(3)));
        }

        private void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            foreach (var item in repo.GetAllItems())
            {
                Console.WriteLine(item);
            }
        }

        private void RunOriginalTests()
        {
            try
            {
                Console.WriteLine("\nAdding duplicate electronic item...");
                _electronics.AddItem(new ElectronicItem(1, "Smartphone", 10, "Samsung", 12));
            }
            catch (DuplicateItemException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            try
            {
                Console.WriteLine("\nRemoving non-existent grocery item...");
                _groceries.RemoveItem(999);
            }
            catch (ItemNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            try
            {
                Console.WriteLine("\nUpdating quantity with invalid value...");
                _electronics.UpdateQuantity(1, -50);
            }
            catch (InvalidQuantityException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        private void InteractiveMenu()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nChoose an action:");
                Console.ResetColor();
                Console.WriteLine("1. Add Electronic Item");
                Console.WriteLine("2. Add Grocery Item");
                Console.WriteLine("3. Update Item Quantity");
                Console.WriteLine("4. Remove Item by ID");
                Console.WriteLine("5. View All Inventory");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddElectronicItem();
                        break;
                    case "2":
                        AddGroceryItem();
                        break;
                    case "3":
                        UpdateItemQuantity();
                        break;
                    case "4":
                        RemoveItem();
                        break;
                    case "5":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n--- Electronics ---");
                        Console.ResetColor();
                        PrintAllItems(_electronics);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n--- Groceries ---");
                        Console.ResetColor();
                        PrintAllItems(_groceries);
                        break;
                    case "0":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Exiting... Thank you for using the system! 👋");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private void AddElectronicItem()
        {
            try
            {
                Console.Write("Enter ID: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter Quantity: ");
                int qty = int.Parse(Console.ReadLine());
                Console.Write("Enter Brand: ");
                string brand = Console.ReadLine();
                Console.Write("Enter Warranty (months): ");
                int warranty = int.Parse(Console.ReadLine());

                _electronics.AddItem(new ElectronicItem(id, name, qty, brand, warranty));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Electronic item added successfully!");
                Console.ResetColor();
            }
            catch (DuplicateItemException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        private void AddGroceryItem()
        {
            try
            {
                Console.Write("Enter ID: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter Quantity: ");
                int qty = int.Parse(Console.ReadLine());
                Console.Write("Enter Expiry Date (dd/MM/yyyy): ");
                DateTime expiry = DateTime.Parse(Console.ReadLine());

                _groceries.AddItem(new GroceryItem(id, name, qty, expiry));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Grocery item added successfully!");
                Console.ResetColor();
            }
            catch (DuplicateItemException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        private void UpdateItemQuantity()
        {
            try
            {
                Console.Write("Enter Item Type (E for Electronic, G for Grocery): ");
                string type = Console.ReadLine().ToUpper();
                Console.Write("Enter Item ID: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Enter New Quantity: ");
                int qty = int.Parse(Console.ReadLine());

                if (type == "E")
                    _electronics.UpdateQuantity(id, qty);
                else if (type == "G")
                    _groceries.UpdateQuantity(id, qty);
                else
                    throw new Exception("Invalid type.");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Quantity updated successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        private void RemoveItem()
        {
            try
            {
                Console.Write("Enter Item Type (E for Electronic, G for Grocery): ");
                string type = Console.ReadLine().ToUpper();
                Console.Write("Enter Item ID: ");
                int id = int.Parse(Console.ReadLine());

                if (type == "E")
                    _electronics.RemoveItem(id);
                else if (type == "G")
                    _groceries.RemoveItem(id);
                else
                    throw new Exception("Invalid type.");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Item removed successfully!");
                Console.ResetColor();
            }
            catch (ItemNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var manager = new WareHouseManager();
            manager.Run();
        }
    }
}
