using System;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using HuInventory;

public class Menu
{


    private User currentUser;
    private bool running = true;
    private Inventory inventory = new Inventory();




    public Menu( User user)
    {
        currentUser = user;

        inventory.LoadInventory();
    }

    public void ShowMainMenu()
    {
        while (running)
        {
            Console.Clear();

            Console.WriteLine($"Logget ind som: {currentUser.Username}");

            Console.WriteLine("=================================");
            Console.WriteLine("           HOVEDMENU");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Lageroversigt");
            Console.WriteLine("2. Bestillingsliste");
            Console.WriteLine("3. Se ændringer i systemet");
            Console.WriteLine("4. Log ud");
            Console.WriteLine("5. Afslut");
            Console.WriteLine("=================================");

            string choice = Console.ReadKey().KeyChar.ToString();

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ShowInventoryMenu();
                    break;

                case "2":
                    ShowOrderList();
                    break;

                case "3":
                    ShowSystemChanges();
                    break;

                case "4":
                    Logout();
                    return;

                case "5":
                    ExitProgram();
                    break;

                default:
                    Console.WriteLine("Ugyldigt valg.");
                    PauseScreen();
                    break;
            }
        }
    }

    private void ShowInventoryMenu()
    {
        bool inventoryMenuRunning = true;

        while (inventoryMenuRunning)
        {
            Console.Clear();

            Console.WriteLine("=================================");
            Console.WriteLine("         LAGEROVERSIGT");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Se alle varer");
            Console.WriteLine("2. Tilføj vare");
            Console.WriteLine("3. Fjern vare");
            Console.WriteLine("4. Tilbage");
            Console.WriteLine("=================================");

            string choice = Console.ReadKey().KeyChar.ToString();

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ShowInventory();
                    break;

                case "2":
                    AddItem();
                    break;

                case "3":
                    RemoveItem();
                    break;

                case "4":
                    inventoryMenuRunning = false;
                    break;

                default:
                    Console.WriteLine("Ugyldigt valg.");
                    PauseScreen();
                    break;
            }
        }
    }

	private void ShowInventory()
	{
		Console.Clear();

		foreach (InventoryItem item in inventory.items)
		{
			Console.WriteLine($"{item.Name} | Antal: {item.GetCurrentInventory()}antal/kg");

			foreach (Batch batch in item.Batches)
			{
				Console.WriteLine(
					$"{batch.Quantity} kg udløber {batch.ExpDate.ToShortDateString()}"
				);
			}

			if (item.GetCurrentInventory() < item.MinInv)
			{
				Console.WriteLine("Varen er under minimumsbeholdning");
			}

			Console.WriteLine();
		}

		PauseScreen();
	}

	private void AddItem()
    {
        Console.Clear();

        Console.WriteLine("Tilføj vare");

        Console.WriteLine("indtast navn på vare; ");
        string name = Console.ReadLine();

        Console.WriteLine("vælg kategori");
        Console.WriteLine("1. tørre vare");
        Console.WriteLine("2. frossen vare");
        string cat = Console.ReadLine();

        InventoryItem.Category category;

        if (cat == "1")
        {
            category = InventoryItem.Category.Dryfoods;
        }

        else
        {
            category = InventoryItem.Category.Frozenfoods;
        }

        Console.WriteLine("Hvor meget af det (kun numre):");
        int quantity = int.Parse(Console.ReadLine());

        Console.WriteLine("Hvad er minimumsantal af varen:");
        int minInv = int.Parse(Console.ReadLine());

        Console.WriteLine("Hvad er udløbsdatoen (yyyy-mm-dd):");
        DateTime expDate = DateTime.Parse(Console.ReadLine());

        inventory.AddItem(name, category, quantity, minInv, expDate);


        PauseScreen();
    }

	private void RemoveItem()
	{
		Console.Clear();

		Console.WriteLine("Vælg vare(tast 1, 2, 3, etc):");


		for (int i = 0; i < inventory.items.Count; i++)
		{
			Console.WriteLine($"{i + 1}. {inventory.items[i].Name}");
		}

		int choice = int.Parse(Console.ReadLine());


		InventoryItem selectedItem = inventory.items[choice - 1];

		Console.Write("Hvor meget har i taget: ");
		int quantity = int.Parse(Console.ReadLine());

		bool yay = inventory.RemoveItem(selectedItem.Name, quantity);

		if (yay)
		{
			Console.WriteLine("lagerindhold opdateret");
		}
		else
		{
			Console.WriteLine("Det var ikke muligt at opdatere lager.");
		}

		PauseScreen();
	}

	private void ShowOrderList()
    {
        Console.Clear();

        Console.WriteLine("Bestillingsliste vises her");

        PauseScreen();
    }

    private void ShowSystemChanges()
    {
        Console.Clear();

        Console.WriteLine("Systemændringer vises her");

        PauseScreen();
    }

    private void Logout()
    {
        Console.WriteLine("Logger ud...");

        PauseScreen();
    }

    private void ExitProgram()
    {
        Console.WriteLine("Programmet afsluttes");

        running = false;

        PauseScreen();
    }

    private void PauseScreen()
    {
        Console.WriteLine("Tryk på en vilkårlig tast for at fortsætte...");

        Console.ReadKey();
    }
}