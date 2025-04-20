using System;
using System.Collections.Generic;
using System.Linq;

public class Transaction
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } // "Income" or "Expense"
    public string Category { get; set; }
    public DateTime Date { get; set; }

    public Transaction(string description, decimal amount, string type, string category, DateTime date)
    {
        Description = description;
        Amount = amount;
        Type = type;
        Category = category;
        Date = date;
    }

    public override string ToString()
    {
        return $"{Date.ToShortDateString()} - {Type} - {Category} - {Description}: ₱{Amount}";
    }
}

public class BudgetTracker
{
    private List<Transaction> transactions = new List<Transaction>();

    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
    }

    public decimal GetTotalIncome() =>
        transactions.Where(t => t.Type.ToLower() == "income").Sum(t => t.Amount);

    public decimal GetTotalExpenses() =>
        transactions.Where(t => t.Type.ToLower() == "expense").Sum(t => t.Amount);

    public decimal GetNetSavings() => GetTotalIncome() - GetTotalExpenses();

    public Dictionary<string, decimal> GetCategorySpending()
    {
        return transactions
            .Where(t => t.Type.ToLower() == "expense")
            .GroupBy(t => t.Category)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
    }

    public void ShowSpendingAnalytics()
    {
        var categorySpending = GetCategorySpending();
        Console.WriteLine("\nCategory-wise Spending:");

        foreach (var category in categorySpending)
        {
            Console.WriteLine($"{category.Key}: ₱{category.Value}");
        }

        if (categorySpending.Count > 0)
        {
            var max = categorySpending.Aggregate((x, y) => x.Value > y.Value ? x : y);
            Console.WriteLine($"\nMost Spent Category: {max.Key} - ₱{max.Value}");
        }
    }

    public void ShowAllTransactions()
    {
        Console.WriteLine("\nAll Transactions:");
        foreach (var transaction in transactions.OrderBy(t => t.Date))
        {
            Console.WriteLine(transaction);
        }
    }

    public void SortTransactions(string sortBy)
    {
        switch (sortBy.ToLower())
        {
            case "date":
                transactions = transactions.OrderBy(t => t.Date).ToList();
                break;
            case "amount":
                transactions = transactions.OrderByDescending(t => t.Amount).ToList();
                break;
            case "category":
                transactions = transactions.OrderBy(t => t.Category).ToList();
                break;
            default:
                Console.WriteLine("Invalid sort option.");
                break;
        }
    }
}

class Program
{
    static void Main()
    {
        BudgetTracker tracker = new BudgetTracker();

        while (true)
        {
            Console.WriteLine("\n--- Personal Budget Tracker ---");
            Console.WriteLine("1. Add Transaction");
            Console.WriteLine("2. View All Transactions");
            Console.WriteLine("3. Show Summary");
            Console.WriteLine("4. Show Spending Analytics");
            Console.WriteLine("5. Sort Transactions");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");

            try
            {
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AddTransaction(tracker);
                        break;
                    case 2:
                        tracker.ShowAllTransactions();
                        break;
                    case 3:
                        Console.WriteLine($"\nTotal Income: ₱{tracker.GetTotalIncome()}");
                        Console.WriteLine($"Total Expenses: ₱{tracker.GetTotalExpenses()}");
                        Console.WriteLine($"Net Savings: ₱{tracker.GetNetSavings()}");
                        break;
                    case 4:
                        tracker.ShowSpendingAnalytics();
                        break;
                    case 5:
                        Console.Write("Sort by (date/amount/category): ");
                        tracker.SortTransactions(Console.ReadLine());
                        tracker.ShowAllTransactions();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void AddTransaction(BudgetTracker tracker)
    {
        try
        {
            Console.Write("Enter Description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Amount: ₱");
            decimal amount = decimal.Parse(Console.ReadLine());

            Console.Write("Enter Type (Income/Expense): ");
            string type = Console.ReadLine();

            Console.Write("Enter Category: ");
            string category = Console.ReadLine();

            Console.Write("Enter Date (yyyy-mm-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            tracker.AddTransaction(new Transaction(description, amount, type, category, date));
            Console.WriteLine("Transaction added successfully!");
        }
        catch
        {
            Console.WriteLine("Invalid input. Transaction not added.");
        }
    }
}
