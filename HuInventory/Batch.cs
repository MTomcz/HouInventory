using System;
using System.Collections.Generic;
using System.Text;

namespace HuInventory
{
	public class Batch
	{
		public int Quantity { get; set; }
		public DateTime ExpDate { get; set; }
		public Batch()
		{

		}



		public Batch(int quantity, DateTime expDate)
		{
			Quantity = quantity;
			ExpDate = expDate;
		}
	}
}
