# 💰 Finance Management System - DCIT 318 Assignment 3 (Question 1)

This C# console application is a **Finance Management System** that demonstrates **records**, **interfaces**, **sealed classes**, and additional interactive functionalities. It allows users to track transactions, process them using different processors, and manage an account with deposit and withdrawal features.

---

## ✅ Features

### 🔹 Core Requirements
- **Records** → Immutable `Transaction` record to store transaction data.
- **Interfaces** → `ITransactionProcessor` implemented by:
  - `BankTransferProcessor`
  - `MobileMoneyProcessor`
  - `CryptoWalletProcessor`
- **Sealed Class** → `SavingsAccount` (cannot be inherited).
- **Polymorphism** → Different processors implement the same interface.
- **Transaction Summary** → Displays all transactions after processing.

### 🔹 Additional Enhancements
- **Deposit Functionality** → User can add funds to the account.
- **Withdraw Functionality** → User can withdraw funds with validation.
- **Dynamic Grocery Transactions** → User can input grocery type and amount.
- **Auto-increment Transaction IDs** for new transactions.
- **Interactive Menu** for deposits, withdrawals, and adding transactions.
- **Colored Console Output** for better user experience.

---
## 💻 How to Run

1. Clone the repository from GitHub or download the source files.
2. Open the project in **Visual Studio** or **Visual Studio Code** with the C# extension installed.
3. Build and run the application.

```bash
cd FinanceManagementSystem

dotnet run