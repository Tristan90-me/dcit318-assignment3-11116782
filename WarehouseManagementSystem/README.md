# 🏬 Warehouse Inventory Management System - DCIT 318 Assignment 3 (Question 3)

This C# console application demonstrates **Generics**, **Custom Exceptions**, and **Collections** by implementing a warehouse system to manage **Electronic Items** and **Grocery Items**. It includes both **original requirements** and **enhanced interactive features**.

---

## ✅ Features

### 🔹 Core Requirements
- **Interface** `IInventoryItem` → defines `Id`, `Name`, and `Quantity`.
- **Generic Repository** `InventoryRepository<T>` with:
  - `AddItem(T item)` → throws `DuplicateItemException`.
  - `GetItemById(int id)` → throws `ItemNotFoundException`.
  - `RemoveItem(int id)` → throws `ItemNotFoundException`.
  - `UpdateQuantity(int id, int newQuantity)` → throws `InvalidQuantityException`.
- **Custom Exceptions**:
  - `DuplicateItemException`
  - `ItemNotFoundException`
  - `InvalidQuantityException`
- **Classes**:
  - `ElectronicItem` → adds `Brand` and `WarrantyMonths`.
  - `GroceryItem` → adds `ExpiryDate`.
- **Exception Tests**:
  - Add duplicate item.
  - Remove non-existent item.
  - Update quantity with invalid value.

### 🔹 Additional Enhancements
- **Interactive Menu** for user actions:
  - Add a new **Electronic Item**.
  - Add a new **Grocery Item**.
  - Update stock quantity for any item.
  - Remove an item by ID.
  - View all inventory items.
- **Dynamic ID and data entry** for items.
- **Validation & Exception Handling** for all user actions.
- **Colored Console Output** for better UX.

---

## 💻 How to Run

1. Clone the repository from GitHub or download the source files.
2. Open the project in **Visual Studio** or **Visual Studio Code** with the C# extension installed.
3. Build and run the application.

```bash
cd WarehouseManagementSystem

dotnet run
