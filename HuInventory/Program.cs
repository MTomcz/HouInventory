while (true)
{
    LoginAuthenticator authservice =
        new LoginAuthenticator();

    bool loggedIn =
        authservice.ShowLoginMenu();

    if (loggedIn)
    {
        Menu menu =
            new Menu(authservice.CurrentUser);

        menu.ShowMainMenu();
    }
}