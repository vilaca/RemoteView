
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class ClickPageHandler : AbstractPageHandler
    {
        private Screen[] screens = Screen.AllScreens;
        private byte last = (byte)' ';

        /// <summary>
        /// Act upon clicks received from application.
        /// Input parsing is robust, trying to work even if problems are present in requesting string.
        /// </summary>
        /// <param name="response">server response</param>
        /// <param name="uri">tokenized URI</param>
        /// <returns></returns>
        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            // must have 5 tokens
            if (uri.Length != 6)
            {
                response.Redirect("/");
                return buildHTML("Error...");
            }

            int x, y;
            try
            {
                y = Convert.ToInt16(uri[4]);
                x = Convert.ToInt16(uri[5]);
            }
            catch
            {
                // parameter error, redirect to home
                response.Redirect("/");
                return buildHTML("Error...");
            }

            int screen = getRequestedScreenDevice(uri, screens);

            // check bounds
            Screen device = screens[screen];
            if (x < 0 || x >= device.Bounds.Width || y < 0 || y >= device.Bounds.Height)
            {
                response.Redirect("/");
                return buildHTML("Error...");
            }

            // grab the first character
            switch (uri[3][0])
            {
                case 'd':
                    if (last == 'd')
                    {
                        // simulate an up
                        LeftMouseButton(MouseEventFlags.MOUSEEVENTF_LEFTUP, x, y);
                    }
                    LeftMouseButton(MouseEventFlags.MOUSEEVENTF_LEFTDOWN, x, y);
                    last = (byte)'d';
                    break;
                case 'u':
                    if (last == 'u')
                    {
                        // simulate a down
                        LeftMouseButton(MouseEventFlags.MOUSEEVENTF_LEFTDOWN, x, y);
                    }
                    LeftMouseButton(MouseEventFlags.MOUSEEVENTF_LEFTUP, x, y);
                    last = (byte)'u';
                    break;
                case 'r':
                    ClickRightMouseButton(x, y);
                    last = (byte)' ';
                    break;
                default:
                    last = (byte)' ';
                    // error, redirect to home
                    response.Redirect("/");
                    return buildHTML("Error...");
            }

            return buildHTML("Updating...");
        }

        enum SystemMetric
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
        }

        #region http://stackoverflow.com/questions/8021954/sendinput-doesnt-perform-click-mouse-button-unless-i-move-cursor
        
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }

        [Flags]
        enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        enum SendInputEventType : int
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }

        struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private int CalculateAbsoluteCoordinateX(int x)
        {
            return (x * 65536) / GetSystemMetrics(SystemMetric.SM_CXSCREEN);
        }

        private int CalculateAbsoluteCoordinateY(int y)
        {
            return (y * 65536) / GetSystemMetrics(SystemMetric.SM_CYSCREEN);
        }

        private void LeftMouseButton(MouseEventFlags mouseEventFlags, int x, int y)
        {
            INPUT mouseInput = new INPUT();
            mouseInput.type = SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;

            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));

            mouseInput.mkhi.mi.dwFlags = mouseEventFlags;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
        }

        private void ClickRightMouseButton(int x, int y)
        {
            INPUT mouseInput = new INPUT();
            mouseInput.type = SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;

            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));

            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));

            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
        }
        #endregion

    }
}
