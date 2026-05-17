using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class LoginAuthenticator
{
    private string filePath = "users.json";

    private List<User> users;

    public User CurrentUser { get; private set; }

    public LoginAuthenticator()
    {
        users = new List<User>();

        LoadUsers();
    }

    public void RegisterUser()
    {
        Console.Clear();

        Console.WriteLine("=================================");
        Console.WriteLine("          OPRET BRUGER");
        Console.WriteLine("=================================");

        Console.Write("Indtast brugernavn: ");
        string username = Console.ReadLine();

        Console.Write("Indtast password: ");
        string password = Console.ReadLine();

        User newUser = new User(username, password);

        users.Add(newUser);

        SaveUsers();

        Console.WriteLine("\nBruger oprettet!");

        Console.ReadKey();
    }

    public bool Login()
    {
        Console.Clear();

        Console.WriteLine("=================================");
        Console.WriteLine("             LOGIN");
        Console.WriteLine("=================================");

        Console.Write("Brugernavn: ");
        string username = Console.ReadLine();

        Console.Write("Password: ");
        string password = Console.ReadLine();

        foreach (User user in users)
        {
            if (user.Username == username &&
                user.Password == password)
            {
                CurrentUser = user;

                Console.WriteLine("\nLogin successfuldt!");

                Console.ReadKey();

                return true;
            }
        }

        Console.WriteLine("\nForkert login.");

        Console.ReadKey();

        return false;
    }

    public bool ShowLoginMenu()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("=================================");
            Console.WriteLine("           LOGIN MENU");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Log ind");
            Console.WriteLine("2. Opret bruger");
            Console.WriteLine("3. Afslut program");
            Console.WriteLine("=================================");

            string choice = Console.ReadKey().KeyChar.ToString();

            Console.WriteLine();

            switch (choice)
            {
                case "1":

                    bool loginSuccess = Login();

                    if (loginSuccess)
                    {
                        return true;
                    }

                    break;

                case "2":

                    RegisterUser();

                    break;

                case "3":

                    running = false;

                    break;

                default:

                    Console.WriteLine("Ugyldigt input.");

                    Console.ReadKey();

                    break;
            }
        }

        return false;
    }

    private void SaveUsers()
    {
        string json =
            JsonSerializer.Serialize(users);

        File.WriteAllText(filePath, json);
    }

    private void LoadUsers()
    {
        if (File.Exists(filePath))
        {
            string json =
                File.ReadAllText(filePath);

            users =
                JsonSerializer.Deserialize<List<User>>(json);
        }
    }
}