using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // Record for Transaction (Immutable)
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // Interface for transaction processing
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // Concrete processors
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Bank Transfer] Processing {transaction.Amount:C} for {transaction.Category}");
            Console.ResetColor();
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Mobile Money] Processing {transaction.Amount:C} for {transaction.Category}");
            Console.ResetColor();
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Crypto Wallet] Processing {transaction.Amount:C} for {transaction.Category}");
            Console.ResetColor();
        }
    }

    // Base Account class
    public class Account
    {
        public string AccountNumber { get; private set; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New balance: {Balance:C}");
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Deposit amount must be positive.");
                Console.ResetColor();
                return;
            }
            Balance += amount;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Deposit successful! New balance: {Balance:C}");
            Console.ResetColor();
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Withdrawal amount must be positive.");
                Console.ResetColor();
                return;
            }

            if (amount > Balance)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Insufficient funds for this withdrawal.");
                Console.ResetColor();
                return;
            }

            Balance -= amount;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Withdrawal successful! New balance: {Balance:C}");
            Console.ResetColor();
        }
    }

    // Sealed SavingsAccount class
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Insufficient funds for this transaction!");
                Console.ResetColor();
            }
            else
            {
                Balance -= transaction.Amount;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Transaction successful. Updated balance: {Balance:C}");
                Console.ResetColor();
            }
        }
    }

    // Main FinanceApp
    public class FinanceApp
    {
        private List<Transaction> _transactions = new List<Transaction>();
        private SavingsAccount _account;

        public void Run()
        {
            Console.Title = "💰 Finance Management System";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Welcome to the Finance Management System ===\n");
            Console.ResetColor();

            // Create SavingsAccount
            _account = new SavingsAccount("ACC-1001", 1000m);
            Console.WriteLine($"Created Savings Account: {_account.AccountNumber} with Balance: {_account.Balance:C}\n");

            // Initial Transactions
            ProcessInitialTransactions();

            // Show transaction summary
            ShowTransactionSummary();

            // Enter interactive mode for deposit, withdrawal, and custom groceries
            EnterInteractiveMode();
        }

        private void ProcessInitialTransactions()
        {
            var transaction1 = new Transaction(1, DateTime.Now, 200m, "Groceries");
            var transaction2 = new Transaction(2, DateTime.Now, 500m, "Utilities");
            var transaction3 = new Transaction(3, DateTime.Now, 400m, "Entertainment");

            ITransactionProcessor mobileMoneyProcessor = new MobileMoneyProcessor();
            ITransactionProcessor bankTransferProcessor = new BankTransferProcessor();
            ITransactionProcessor cryptoWalletProcessor = new CryptoWalletProcessor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Processing Initial Transactions ---\n");
            Console.ResetColor();

            mobileMoneyProcessor.Process(transaction1);
            _account.ApplyTransaction(transaction1);
            _transactions.Add(transaction1);

            bankTransferProcessor.Process(transaction2);
            _account.ApplyTransaction(transaction2);
            _transactions.Add(transaction2);

            cryptoWalletProcessor.Process(transaction3);
            _account.ApplyTransaction(transaction3);
            _transactions.Add(transaction3);
        }

        private void ShowTransactionSummary()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- Transaction Summary ---");
            foreach (var t in _transactions)
            {
                Console.WriteLine($"ID: {t.Id}, Date: {t.Date}, Amount: {t.Amount:C}, Category: {t.Category}");
            }
            Console.ResetColor();
        }

        private void EnterInteractiveMode()
        {
            int customId = _transactions.Count + 1;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nChoose an action:");
                Console.ResetColor();
                Console.WriteLine("1. Deposit Money");
                Console.WriteLine("2. Withdraw Money");
                Console.WriteLine("3. Add Grocery Transaction");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter amount to deposit: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                        {
                            _account.Deposit(depositAmount);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid amount.");
                            Console.ResetColor();
                        }
                        break;

                    case "2":
                        Console.Write("Enter amount to withdraw: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                        {
                            _account.Withdraw(withdrawAmount);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid amount.");
                            Console.ResetColor();
                        }
                        break;

                    case "3":
                        Console.Write("Enter grocery category (e.g., Fruits, Vegetables): ");
                        string category = Console.ReadLine();
                        Console.Write("Enter amount spent: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal groceryAmount))
                        {
                            var groceryTransaction = new Transaction(customId++, DateTime.Now, groceryAmount, category);
                            _transactions.Add(groceryTransaction);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Grocery transaction added: {category} - {groceryAmount:C}");
                            Console.ResetColor();
                            _account.ApplyTransaction(groceryTransaction);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid amount.");
                            Console.ResetColor();
                        }
                        break;

                    case "0":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Thank you for using the Finance Management System! 👋");
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            var app = new FinanceApp();
            app.Run();
        }
    }
}
