using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static HuInventory.InventoryItem;

using System.IO;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;

namespace HuInventory
{
    internal class Inventory
    {

        private string filePath = "inventory.json";

        public List<InventoryItem> items = new List<InventoryItem>();

		public Inventory()
		{
			LoadInventory();
		}

        public void AddItem(string name, InventoryItem.Category category, int quantity, int minInv, DateTime expDate)
        {
            bool alrExist = false;


            foreach (InventoryItem item in items)
            {

                if (item.Name.ToLower() == name.ToLower())
                {
                    item.AddStock(quantity, expDate);
					SaveInventory();

                    alrExist = true;
                    break;

                }
         

            }

            if (!alrExist)
            {
                InventoryItem newItem = new InventoryItem(name, category, minInv);


                newItem.AddStock(quantity, expDate);

                items.Add(newItem);
				SaveInventory();

            }


	    }

		public bool RemoveItem(string name, int quantity)
		{
			foreach (InventoryItem item in items)
			{
				if (item.Name.ToLower() == name.ToLower())
				{
					bool removed = item.RemoveStock(quantity);
					SaveInventory();
					return removed;
				}
			}

			return false;
		}

		public void SaveInventory()
		{

			string invJson = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });   // Serialize the inventory list to JSON with indentation for readability

            File.WriteAllText(filePath, invJson);

		}

		public void LoadInventory()
		{
			if (File.Exists(filePath))
			{
				string invJson = File.ReadAllText(filePath);

				var loaded = JsonSerializer.Deserialize<List<InventoryItem>>(invJson);

				if (loaded != null)
				{
					items = loaded;
				}
				else
				{
					items = new List<InventoryItem>();
				}
			}
		}

		public List<InventoryItem> GetLowStockItems()
		{
			List<InventoryItem> lowStockitems = new List<InventoryItem>();

			foreach (InventoryItem item in items)
			{
				if (item.GetCurrentInventory() <= item.MinInv)
				{
					lowStockitems.Add(item);

				}
			}
			return lowStockitems;
		}

	}

}
