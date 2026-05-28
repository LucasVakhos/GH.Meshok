//#define WITH_LOG
namespace GH.Components
{
    using EventHook;
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.Windows.Forms;
    using static GH.Components.WM_Messages;
    [SuppressUnmanagedCodeSecurity]
    public class MessageHook : IMessageFilter, IDisposable
    {
        static ApplicationWatcher _appWatcher;
        static ClipboardWatcher _clipWatcher;
        static EventHookFactory _hookFactory = new EventHookFactory();
        static MessageHook _hook = null;
        static List<ActionList> _queue = new List<ActionList>();
#if WITH_LOG
        static string _log = "";
#endif
        static MessageHook()
        {
            _appWatcher = _hookFactory.GetApplicationWatcher();
            _appWatcher.OnApplicationWindowChange += ApplicationWindowChange;
            _appWatcher.Start();
            _clipWatcher = _hookFactory.GetClipboardWatcher();
            _clipWatcher.OnClipboardModified += ClipboardModified;
            _clipWatcher.Start();
            _hook = new MessageHook();
        }
        private static void ClipboardModified(object sender, ClipboardEventArgs e)
        {
            Console.WriteLine("Clipboard updated with data '{0}' of format {1}", e.Data,
                                e.DataFormat.ToString());
        }
        private static void ApplicationWindowChange(object sender, ApplicationEventArgs e)
        {
            if (e.Event == ApplicationEvents.Launched && !RunContext.AppRunning)
            {
                Application.ApplicationExit += Application_ApplicationExit;
                if (RunContext.AppMainForm != null)
                {
                    RunContext.AppRunning = true;
                }
            }
        }
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Destroy();
        }
        private static void Destroy()
        {
            _queue.Clear();
            _hook.Dispose();
            _hook = null;
            _clipWatcher.Stop();
            _appWatcher.Stop();
            _hookFactory.Dispose();
            _clipWatcher = null;
            _appWatcher = null;
            _hookFactory = null;
#if WITH_LOG
            File.WriteAllText("log.txt", _log);
            _log = null;
#endif
        }
        public static void RegForQueue(ActionList list)
        {
            if (_queue.Contains(list))
                return;
            _queue.Add(list);
        }
        public static void UnRegQueue(ActionList list)
        {
            if (_queue.Contains(list))
                _queue.Remove(list);
        }
        internal class InnerKeyEventArgs : KeyEventArgs
        {
            public InnerKeyEventArgs(Keys keyData) : base(keyData)
            {
            }
            private bool Modifier => Alt || Shift || Control;
            private bool HotKey
            {
                get
                {
                    for (Keys i = Keys.F1; i <= Keys.F12; i++)
                        if ((i & KeyData) == i)
                            return true;
                    return false;
                }
            }
            private bool LetterKey
            {
                get
                {
                    if (KeyCode == Keys.ShiftKey || KeyCode == Keys.ControlKey || KeyCode == Keys.Menu)
                        return false;
                    return true;
                }
            }
            public bool IsChortcut
            {
                get
                {
                    if ((LetterKey && Modifier) || HotKey)
                        return true;
                    return false;
                }
            }
        }
        public MessageHook()
        {
            Application.AddMessageFilter(this);
        }
        private void WmKeyDownUp(ref Message m)
        {
            if (m.Msg == (int)WM_KEYDOWN || m.Msg == (int)WM_SYSKEYDOWN)
            {
                if (_queue == null)
                    return;
                InnerKeyEventArgs e = new InnerKeyEventArgs((Keys)(long)m.WParam | Control.ModifierKeys);
                if (e.IsChortcut)
                {
                    foreach (ActionList item in _queue.FindAll(x => x.Active))
                        item.CheckShortcuts(item, e);
                }
            }
            else
            {
            }
        }
        private void Update()
        {
            foreach (ActionList item in _queue.FindAll(x => x.Active))
                item.DoUpdate();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public bool PreFilterMessage(ref Message m)
        {
#if WITH_LOG
            switch ((GH.Components.WM_Messages)m.Msg)
            {
                case WM_TIMER:
                case WM_PAINT:
                case WM_NCACTIVATE:
                case WM_MOUSEMOVE:
                case WM_MOUSELEAVE:
                case WM_MOUSEHOVER:
                case WM_LBUTTONDOWN:
                case WM_LBUTTONUP:
                case WM_NCMOUSEMOVE:
                case WM_NCLBUTTONDOWN:
                case WM_NCMBUTTONDOWN:
                case WM_NCMBUTTONUP:
                case WM_NCMBUTTONDBLCLK:
                case WM_NCXBUTTONDOWN:
                case WM_NCXBUTTONUP:
                case WM_NCXBUTTONDBLCLK:                    
                    return false;
                default:
                    break;
            }
            _log += $" m.Msg {m.Msg} m.LParam {m.LParam} ({m.ToString()})\r\n";
#endif
            switch ((GH.Components.WM_Messages)m.Msg)
            {
                case WM_KEYDOWN:
                case WM_KEYUP:
                case WM_SYSKEYDOWN:
                case WM_SYSKEYUP:
                    WmKeyDownUp(ref m);
                    break;
                default:
                    Update();
                    break;
            }
            return false;
        }
    }
}
