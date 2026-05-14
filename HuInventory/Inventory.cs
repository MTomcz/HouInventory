using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static HuInventory.InventoryItem;

namespace HuInventory
{
    internal class Inventory
    {
        public List<InventoryItem> items = new List<InventoryItem>();

        public void AddItem(string name, InventoryItem.Category category, int quantity, int minInv, DateTime expDate)
        {
            bool alrExist = false;


            foreach (InventoryItem item in items)
            {

                if (item.Name == name)
                {
                    item.AddStock(quantity, expDate);

                    alrExist = true;
                    break;

                }
         

            }

            if (!alrExist)
            {
                InventoryItem newItem = new InventoryItem(name, category, minInv);


                newItem.AddStock(quantity, expDate);

                items.Add(newItem);

            }


        }



    }

}
