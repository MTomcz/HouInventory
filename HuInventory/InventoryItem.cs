using System;
using System.Collections.Generic;
using System.Text;

namespace HuInventory
{
    internal class InventoryItem
    {
		public enum Category
		{
			Dryfoods,
			Frozenfoods,

		}
		public string Name { get; set; }
		public Category ItemCategory { get; set; }
		public int MinInv { get; set; }

		public List<Batch> Batches { get; set; } = new List<Batch>();


		public InventoryItem(string name, Category category, int minInv)
		{
			Name = name;
			ItemCategory = category;
			MinInv = minInv;

		}

		public void AddStock(int quantity, DateTime expDate)
		{
			if (quantity <= 0)
				return;

			Batches.Add(new Batch(quantity, expDate));
		}


		public int GetCurrentInventory()
		{
			int total = 0;

			foreach (Batch batch in Batches)
			{
				total += batch.Quantity;
			}

			return total;
		}


	}
}
