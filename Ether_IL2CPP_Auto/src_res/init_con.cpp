VOID InitConsole()
{
    AllocConsole();
    freopen_s((FILE **)stdout, "CONOUT$", "w", stdout);
    freopen_s((FILE **)stderr, "CONOUT$", "w", stderr);
    auto consoleWindow = GetConsoleWindow();
    SetForegroundWindow(consoleWindow);
    ShowWindow(consoleWindow, SW_RESTORE);
    ShowWindow(consoleWindow, SW_SHOW);
}