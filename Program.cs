using System.Runtime.InteropServices;

internal class Program
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern nint FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", SetLastError = true)]
    static extern nint GetWindow(nint hWnd, GetWindow_Cmd uCmd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);
    // https://stackoverflow.com/a/56812642
    static void ToggleDesktopIcons()
    {
        var toggleDesktopCommand = new nint(0x7402);
        SendMessage(GetDesktopSHELLDLL_DefView(), WM_COMMAND, toggleDesktopCommand, nint.Zero);
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern nint FindWindowEx(nint parentHandle, nint childAfter, string className, string? windowTitle);
    [DllImport("user32.dll", SetLastError = false)]
    static extern nint GetDesktopWindow();

    static nint GetDesktopSHELLDLL_DefView()
    {
        var hShellViewWin = nint.Zero;
        var hWorkerW = nint.Zero;

        var hProgman = FindWindow("Progman", "Program Manager");
        var hDesktopWnd = GetDesktopWindow();

        // If the main Program Manager window is found
        if (hProgman != nint.Zero)
        {
            // Get and load the main List view window containing the icons.
            hShellViewWin = FindWindowEx(hProgman, nint.Zero, "SHELLDLL_DefView", null);
            if (hShellViewWin == nint.Zero)
            {
                // When this fails (picture rotation is turned ON, toggledesktop shell cmd used ), then look for the WorkerW windows list to get the
                // correct desktop list handle.
                // As there can be multiple WorkerW windows, iterate through all to get the correct one
                do
                {
                    hWorkerW = FindWindowEx(hDesktopWnd, hWorkerW, "WorkerW", null);
                    hShellViewWin = FindWindowEx(hWorkerW, nint.Zero, "SHELLDLL_DefView", null);
                } while (hShellViewWin == nint.Zero && hWorkerW != nint.Zero);
            }
        }
        return hShellViewWin;
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

