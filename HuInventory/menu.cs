using System;

public class Menu
{
    private User currentUser;
    private bool running = true;
    public Menu( User user)
    {
        currentUser = user;
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

        Console.WriteLine("Alle varer vises her...");

        PauseScreen();
    }

    private void AddItem()
    {
        Console.Clear();

        Console.WriteLine("Tilføj vare");

        PauseScreen();
    }

    private void RemoveItem()
    {
        Console.Clear();

        Console.WriteLine("Fjern vare");

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
        Console.WriteLine("\nTryk på en vilkårlig tast for at fortsætte...");

        Console.ReadKey();
    }
}