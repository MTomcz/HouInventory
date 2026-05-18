using System;

namespace HuInventory
{
    internal class StockTransaction
    {
        public string Username { get; set; }
        public DateTime Timestamp { get; set; }
        public string TransactionType { get; set; } // "Login", "Logout", "AddStock", "RemoveStock"
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public StockTransaction(string username, string transactionType, string itemName = "", int quantity = 0)
        {
            Username = username;
            Timestamp = DateTime.Now;
            TransactionType = transactionType;
            ItemName = itemName;
            Quantity = quantity;
        }
    }
}
