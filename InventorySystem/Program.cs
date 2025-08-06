using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventorySystem
{
    // Interface for inventory entity
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Immutable record for inventory item
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    // Generic logger for inventory
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private List<T> _log = new List<T>();
        private string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(_log);
        }

        public void SaveToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Data saved successfully to {_filePath}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error saving data: {ex.Message}");
                Console.ResetColor();
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("No previous data file found. Starting with an empty list.");
                    Console.ResetColor();
                    return;
                }

                string json = File.ReadAllText(_filePath);
                var data = JsonSerializer.Deserialize<List<T>>(json);

                if (data != null)
                {
                    _log = data;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Data loaded successfully from {_filePath}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error loading data: {ex.Message}");
                Console.ResetColor();
            }
        }
    }

    // Main Inventory App
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;

        public InventoryApp(string filePath)
        {
            _logger = new InventoryLogger<InventoryItem>(filePath);
        }

        public void Run()
        {
            Console.Title = "📦 Inventory Management System";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Welcome to the Inventory Management System ===\n");
            Console.ResetColor();

            _logger.LoadFromFile();

            if (_logger.GetAll().Count == 0)
            {
                SeedSampleData();
                Console.WriteLine("Sample data has been added.");
            }

            InteractiveMenu();
        }

        private void SeedSampleData()
        {
            _logger.Add(new InventoryItem(1, "Rice Bag", 50, DateTime.Now));
            _logger.Add(new InventoryItem(2, "Cooking Oil", 20, DateTime.Now));
            _logger.Add(new InventoryItem(3, "Sugar Pack", 30, DateTime.Now));
            _logger.SaveToFile();
        }

        private void InteractiveMenu()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nChoose an action:");
                Console.ResetColor();
                Console.WriteLine("1. View All Inventory");
                Console.WriteLine("2. Add New Item");
                Console.WriteLine("3. Save Data");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        PrintAllItems();
                        break;

                    case "2":
                        AddNewItem();
                        break;

                    case "3":
                        _logger.SaveToFile();
                        break;

                    case "0":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Exiting... Goodbye! 👋");
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

        private void PrintAllItems()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Inventory List ---");
            Console.ResetColor();

            var items = _logger.GetAll();
            if (items.Count == 0)
            {
                Console.WriteLine("No inventory items found.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Added: {item.DateAdded}");
            }
        }

        private void AddNewItem()
        {
            try
            {
                Console.Write("Enter Item ID: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Enter Item Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                var newItem = new InventoryItem(id, name, quantity, DateTime.Now);
                _logger.Add(newItem);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Item added successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error adding item: {ex.Message}");
                Console.ResetColor();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "inventory.json";
            var app = new InventoryApp(filePath);
            app.Run();
        }
    }
}
