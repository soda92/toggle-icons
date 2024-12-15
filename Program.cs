using System.Runtime.InteropServices;

internal class Program
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern nint FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", SetLastError = true)]
    static extern nint GetWindow(nint hWnd, GetWindow_Cmd uCmd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);
    static void ToggleDesktopIcons()
    {
        var toggleDesktopCommand = new nint(0x7402);
        nint hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
        SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, nint.Zero);
    }
    private const int WM_COMMAND = 0x111;
    private static void Main(string[] args)
    {
        ToggleDesktopIcons();
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");
    }
}

enum GetWindow_Cmd : uint
{
    GW_HWNDFIRST = 0,
    GW_HWNDLAST = 1,
    GW_HWNDNEXT = 2,
    GW_HWNDPREV = 3,
    GW_OWNER = 4,
    GW_CHILD = 5,
    GW_ENABLEDPOPUP = 6
}

