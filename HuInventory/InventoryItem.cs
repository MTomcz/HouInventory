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
		public int Amount { get; set; }
		public int CurrentInv { get; set; }
		public int MinInv { get; set; }
		public DateTime ExpDate { get; set; }

		public InventoryItem(string name, Category category, int amount, int currentInv, int minInv, DateTime expDate)
		{
			Name = name;
			ItemCategory = category;
			Amount = amount;
			CurrentInv = currentInv;
			MinInv = minInv;
			ExpDate = expDate;

		}

		public void AddStock(int quantity)
		{

			if (quantity <= 0)
			{
				return;
			}

			CurrentInv += quantity;
		}

		public bool RemoveStock(int quantity)
		{
			if (quantity <= 0)
			{
				return false;
			}

			if (quantity > CurrentInv)
			{
				return false;
			}

			CurrentInv -= quantity;
			return true;

		}



	}
}
