# 📦 Inventory Management System - DCIT 318 Assignment 3 (Question 5)

This C# console application demonstrates the use of **records**, **interfaces**, **generics**, and **file operations** to build a persistent and modular inventory management system.

---

## ✅ Features

### 🔹 Core Requirements
- **Immutable record** `InventoryItem` with:
  - `Id`, `Name`, `Quantity`, `DateAdded`.
- **Interface** `IInventoryEntity` with property `Id`.
- **Generic Logger Class** `InventoryLogger<T>`:
  - `Add(T item)`
  - `GetAll()`
  - `SaveToFile()` → persists data to a file in JSON format.
  - `LoadFromFile()` → restores data from file.
- **Exception Handling** for:
  - Missing file
  - Corrupted file
  - Generic file I/O errors
- **Class** `InventoryApp`:
  - `SeedSampleData()`
  - `PrintAllItems()`
  - `AddNewItem()`
  - `SaveData()`
  - `InteractiveMenu()`

### 🔹 Enhancements
- ✅ Automatically seeds inventory if no data file exists.
- ✅ Console menu for real-time user interaction.
- ✅ Validation for user input.
- ✅ Uses JSON file (`inventory.json`) for persistent storage.
- ✅ Colored console output for improved UX.

---
## 💻 How to Run

1. Clone the repository from GitHub or download the source files.
2. Open the project in **Visual Studio** or **Visual Studio Code** with the C# extension installed.
3. Build and run the application.

```bash
cd InventorySystem

dotnet run


