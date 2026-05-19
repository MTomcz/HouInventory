using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using HuInventory;

public class SystemChanges
{
    private string filePath = "systemchanges.json";
    private List<SystemLog> transactions = new List<SystemLog>();

    public SystemChanges()
    {
        LoadChanges();
    }

    public void LogTransaction(string username, string transactionType, string itemName = "", int quantity = 0)
    {
        SystemLog transaction = new SystemLog(username, transactionType, itemName, quantity);
        transactions.Add(transaction);
        SaveChanges();
    }

    public void SaveChanges()
    {
        string json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void LoadChanges()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var loaded = JsonSerializer.Deserialize<List<SystemLog>>(json);
            transactions = loaded ?? new List<SystemLog>();
        }
    }

    public List<SystemLog> GetAllTransactions()
    {
        return transactions;
    }

    // Filter methods
    public List<SystemLog> FilterByUsername(string username)
    {
        return transactions.Where(t => t.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<SystemLog> FilterByTransactionType(string transactionType)
    {
        return transactions.Where(t => t.TransactionType.Equals(transactionType, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<SystemLog> FilterByDateRange(DateTime startDate, DateTime endDate)
    {
        return transactions.Where(t => t.Timestamp.Date >= startDate.Date && t.Timestamp.Date <= endDate.Date).ToList();
    }

    public List<SystemLog> SearchByItemName(string itemName)
    {
        return transactions.Where(t => !string.IsNullOrEmpty(t.ItemName) && t.ItemName.Contains(itemName, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void DisplayAllChanges()
    {
        DisplayFilteredChanges(transactions);
    }

    public void DisplayFilteredChanges(List<SystemLog> logs)
    {
        Console.Clear();
        Console.WriteLine("========== SYSTEMÆNDRINGER ==========\n");

        if (logs.Count == 0)
        {
            Console.WriteLine("Ingen ændringer registreret.");
        }
        else
        {
            foreach (var transaction in logs)
            {
                Console.WriteLine($"Bruger: {transaction.Username}");
                Console.WriteLine($"Tidspunkt: {transaction.Timestamp:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine($"Handling: {transaction.TransactionType}");

                if (!string.IsNullOrEmpty(transaction.ItemName))
                {
                    Console.WriteLine($"Vare: {transaction.ItemName} (Mængde: {transaction.Quantity})");
                }

                Console.WriteLine("-----------------------------------");
            }
        }

        Console.WriteLine("\nTryk på en tast for at vende tilbage...");
        Console.ReadKey();
    }
}

