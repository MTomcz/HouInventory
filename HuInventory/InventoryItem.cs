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

		public bool RemoveStock(int quantity)
		{
			if (quantity <= 0)
			{
				return false;
			}

			if (quantity > GetCurrentInventory())
			{
				return false;
			}

			int remove = quantity;

			foreach (Batch batch in Batches)
			{
				if (remove == 0)
				{
					break;
				}

				if (batch.Quantity >= remove)
				{
					batch.Quantity = batch.Quantity - remove;

					remove = 0;
				}
				else
				{
					remove = remove - batch.Quantity;

					batch.Quantity = 0;
				}
			}

			for (int i = Batches.Count - 1; i >= 0; i--)
			{
				if (Batches[i].Quantity == 0)
				{
					Batches.RemoveAt(i);
				}
			}

			return true;
		}


	}
}
