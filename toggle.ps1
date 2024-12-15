$def = @'
[DllImport("user32.dll", SetLastError = true)]
static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
[DllImport("user32.dll", SetLastError = true)]
static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);
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
[DllImport("user32.dll", CharSet = CharSet.Auto)] static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

private const int WM_COMMAND = 0x111;

static void ToggleDesktopIcons()
{
	var toggleDesktopCommand = new IntPtr(0x7402);
	IntPtr hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
	SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
}
'@
$Kernel32 = Add-Type -MemberDefinition $def -Name 'Kernel32' -Namespace 'Win32' -PassThru

$Kernel32::ToggleDesktopIcons()
