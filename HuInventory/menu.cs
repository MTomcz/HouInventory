using System;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using HuInventory;

public class Menu
{


    private User currentUser;
    private bool running = true;
    private Inventory inventory = new Inventory();
    private SystemChanges systemChanges = new SystemChanges();



    public Menu( User user)
    {
        currentUser = user;

        inventory.LoadInventory();
        systemChanges.LogTransaction(user.Username, "Login");
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
            Console.WriteLine($"{item.Name} | " + $"Kategori: {item.ItemCategory} | " + $"Antal: {item.GetCurrentInventory()} antal/kg");
  

            foreach (Batch batch in item.Batches)
			{
				Console.WriteLine(
					$"{batch.Quantity} kg udløber {batch.ExpDate.ToShortDateString()}"
				);
			}

            if (item.GetCurrentInventory() <= item.MinInv)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("ADVARSEL! Varen er under minimumsbeholdning");

                Console.ResetColor();
            }

            Console.WriteLine();
		}

		PauseScreen();
	}

	private void AddItem()
	{
		try
		{
			Console.Clear();

			Console.WriteLine("Tilføj vare");

			Console.WriteLine("indtast navn på vare; ");
			string name = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(name))
			{
				Console.WriteLine("Varenavn kan ikke være tomt.");
				PauseScreen();
				return;
			}

			Console.WriteLine("vælg kategori");
			Console.WriteLine("1. tørre vare");
			Console.WriteLine("2. frossen vare");
			string cat = Console.ReadLine();

			InventoryItem.Category category;

            if (cat == "1")
            {
                category = InventoryItem.Category.Dryfoods;
            }

            else if (cat == "2")
            {
                category = InventoryItem.Category.Frozenfoods;
            }

            else
            {
                Console.WriteLine("Skriv kun 1 eller 2");
                PauseScreen();
                return;
            }

            Console.WriteLine("Hvor meget af det (kun numre):");
			if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
			{
				Console.WriteLine("Ugyldigt antal. Skal være et positivt tal.");
				PauseScreen();
				return;
			}

			Console.WriteLine("Hvad er minimumsantal af varen:");
			if (!int.TryParse(Console.ReadLine(), out int minInv) || minInv < 0)
			{
				Console.WriteLine("Ugyldigt minimumsantal. Skal være 0 eller højere.");
				PauseScreen();
				return;
			}

			Console.WriteLine("Hvad er udløbsdatoen (yyyy-mm-dd):");
			if (!DateTime.TryParse(Console.ReadLine(), out DateTime expDate))
			{
				Console.WriteLine("Ugyldig dato. Brug formatet yyyy-mm-dd.");
				PauseScreen();
				return;
			}

			inventory.AddItem(name, category, quantity, minInv, expDate);
			systemChanges.LogTransaction(currentUser.Username, "AddStock", name, quantity);

			Console.WriteLine("Vare tilføjet succesfuldt!");
			PauseScreen();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Fejl: {ex.Message}");
			PauseScreen();
		}
	}

	private void RemoveItem()
	{
		try
		{
			Console.Clear();

			Console.WriteLine("Vælg vare(tast 1, 2, 3, etc):");

			// Show all items
			for (int i = 0; i < inventory.items.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {inventory.items[i].Name}");
			}

			if (inventory.items.Count == 0)
			{
				Console.WriteLine("Der er ingen varer i lageret.");
				PauseScreen();
				return;
			}

			if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > inventory.items.Count)
			{
				Console.WriteLine("Ugyldigt valg.");
				PauseScreen();
				return;
			}

			InventoryItem selectedItem = inventory.items[choice - 1];

			Console.Write("Hvor meget har i taget: ");
			if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
			{
				Console.WriteLine("Ugyldigt antal. Skal være et positivt tal.");
				PauseScreen();
				return;
			}

			bool yay = inventory.RemoveItem(selectedItem.Name, quantity);

			if (yay)
			{
				Console.WriteLine("lagerindhold er opdateret");
				systemChanges.LogTransaction(currentUser.Username, "RemoveStock", selectedItem.Name, quantity);
			}
			else
			{
				Console.WriteLine("Det var ikke muligt at opdatere lager.");
			}

			PauseScreen();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Fejl: {ex.Message}");
			PauseScreen();
		}
	}

	private void ShowOrderList()
    {
        Console.Clear();

        Console.WriteLine("=================================");
        Console.WriteLine("        BESTILLINGSLISTE");
        Console.WriteLine("=================================");

        List<InventoryItem> lowStockItems = inventory.GetLowStockItems();

        if (lowStockItems.Count == 0)
        {
            Console.WriteLine(" Ingen varer mangler.");
        }
        else
        {
            {
                foreach (InventoryItem item in lowStockItems)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ADVARSEL! {item.Name} | " + $"Nuværende: {item.GetCurrentInventory()} | " + $"Minimum: {item.MinInv}");


                    Console.ResetColor();

                }
            }
        }


            PauseScreen();
    }

    private void ShowSystemChanges()
    {
        bool changesMenuRunning = true;

        while (changesMenuRunning)
        {
            Console.Clear();

            Console.WriteLine("=================================");
            Console.WriteLine("      SYSTEMÆNDRINGER - FILTER");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Se alle ændringer");
            Console.WriteLine("2. Filtrer efter bruger");
            Console.WriteLine("3. Filtrer efter type");
            Console.WriteLine("4. Filtrer efter dato");
            Console.WriteLine("5. Søg efter vare");
            Console.WriteLine("6. Tilbage");
            Console.WriteLine("=================================");

            string choice = Console.ReadKey().KeyChar.ToString();

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    systemChanges.DisplayAllChanges();
                    break;

                case "2":
                    FilterByUsername();
                    break;

                case "3":
                    FilterByTransactionType();
                    break;

                case "4":
                    FilterByDateRange();
                    break;

                case "5":
                    SearchByItemName();
                    break;

                case "6":
                    changesMenuRunning = false;
                    break;

                default:
                    Console.WriteLine("Ugyldigt valg.");
                    PauseScreen();
                    break;
            }
        }
    }

    private void FilterByUsername()
    {
        Console.Clear();
        Console.WriteLine("Indtast brugernavn:");
        string username = Console.ReadLine();

        var filtered = systemChanges.FilterByUsername(username);
        systemChanges.DisplayFilteredChanges(filtered);
    }

    private void FilterByTransactionType()
    {
        Console.Clear();
        Console.WriteLine("=================================");
        Console.WriteLine("Vælg transaktionstype:");
        Console.WriteLine("=================================");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Logout");
        Console.WriteLine("3. AddStock");
        Console.WriteLine("4. RemoveStock");
        Console.WriteLine("=================================");

        string choice = Console.ReadKey().KeyChar.ToString();
        string transactionType = "";

        switch (choice)
        {
            case "1":
                transactionType = "Login";
                break;
            case "2":
                transactionType = "Logout";
                break;
            case "3":
                transactionType = "AddStock";
                break;
            case "4":
                transactionType = "RemoveStock";
                break;
            default:
                Console.WriteLine("Ugyldigt valg.");
                PauseScreen();
                return;
        }

        var filtered = systemChanges.FilterByTransactionType(transactionType);
        systemChanges.DisplayFilteredChanges(filtered);
    }

    private void FilterByDateRange()
    {
        Console.Clear();
        Console.WriteLine("Filtrer efter dato");

        Console.WriteLine("Indtast startdato (yyyy-mm-dd):");
        DateTime startDate = DateTime.Parse(Console.ReadLine());

        Console.WriteLine("Indtast slutdato (yyyy-mm-dd):");
        DateTime endDate = DateTime.Parse(Console.ReadLine());

        var filtered = systemChanges.FilterByDateRange(startDate, endDate);
        systemChanges.DisplayFilteredChanges(filtered);
    }

    private void SearchByItemName()
    {
        Console.Clear();
        Console.WriteLine("Søg efter vare:");
        string itemName = Console.ReadLine();

        var filtered = systemChanges.SearchByItemName(itemName);
        systemChanges.DisplayFilteredChanges(filtered);
    }

    private void Logout()
    {
        systemChanges.LogTransaction(currentUser.Username, "Logout");
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