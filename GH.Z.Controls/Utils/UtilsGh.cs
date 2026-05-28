using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors.Repository;
using GH.Configs;

namespace GH.Utils
{
    public static class UtilsGh
    {
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);


        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static async void WaitAndKeysendWindowAsinc(string win_title, string sent_text)
        {
            int wait_step = 0;
            while (wait_step < 1000)
            {
                await Task.Delay(50);
                IntPtr DialogHandle = FindWindow(null, win_title);
                if (DialogHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(DialogHandle);
                    if (sent_text == "")
                        SendKeys.Send("{ENTER}");
                    else
                    {
                        await Task.Delay(100);
                        SendKeys.Send(sent_text + "{ENTER}");
                    }
                    break;
                }
                wait_step++;
            }
        }

        public static void SaveTextToFile(string text, string filePath, bool openDir = false)
        {
            string dir = Path.Combine(Application.StartupPath, IniHelper.CfgAppForm().ExportPath);
            string ext = Path.GetExtension(filePath);
            if (ext == null)
            {
                ext = ".txt";
            }

            filePath = Path.GetFileNameWithoutExtension(filePath) + ext;

            filePath = Path.Combine(dir, filePath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(filePath, text, Encoding.GetEncoding(1251));

            if (!File.Exists(filePath) || !openDir)
            {
                return;
            }

            OpenDirrectory(filePath);
        }

        public static void OpenDirrectory(string filePath)
        {
            Process process = new Process();
            ProcessStartInfo info = new ProcessStartInfo("Explorer", " /select, " + filePath);
            info.WorkingDirectory = Path.GetDirectoryName(filePath);
            process.StartInfo = info;
            process.Start();
        }

        public static string FormatAdress(string address)
        {
            address = " " + address.Replace(" .", ".").Replace(".", ". ").Replace("  ", " ");
            string[] comaChars = { "," };
            string[] splitComa = address.Split(comaChars, StringSplitOptions.RemoveEmptyEntries);
            address = "";
            foreach (string textComa in splitComa)
            {
                string[] dotChars = { ".", " " };
                string[] splitDot = textComa.Split(dotChars, StringSplitOptions.RemoveEmptyEntries);
                string clearTextComa = textComa;
                string newTextComa = "";

                bool toUp = false;
                foreach (string item in splitDot)
                {
                    int pos = clearTextComa.IndexOf(" " + item);
                    int posDot = clearTextComa.IndexOf(" " + item + ".");

                    if (posDot >= 0)
                    {
                        newTextComa += item + ". ";
                        toUp = true;
                    }
                    else
                    if (pos >= 0)
                    {
                        if (toUp)
                        {
                            newTextComa += item.WordToProperCase() + " ";
                            toUp = false;
                        }
                        else
                            newTextComa += item + " ";
                    }


                }

                if (address == "")
                    address = newTextComa.Trim();
                else
                    address += ", " + clearTextComa.Trim();
            }
            return address.Trim();
        }


        public static bool IsZip(string zip)
        {
            if (zip.Length == 6)
            {
                string pattern = "[0-9]{6}";
                Match isMatch = Regex.Match(zip, pattern, RegexOptions.IgnoreCase);

                return isMatch.Success;
            }
            else
                return false;
        }

        public static bool IsPhone(string phone)
        {

            string pattern = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
            Match isMatch = Regex.Match(phone, pattern, RegexOptions.IgnoreCase);

            return isMatch.Success;
        }

        public static void SetupLookups(object item)
        {
            if (item == null)
                return;

            if (item is GridControl gridControl)
            {
                foreach (object repo in gridControl.RepositoryItems)
                {
                    SetupLookups(repo);
                }

            }
            else
            if (item is RepositoryItemLookUpEdit repositoryItemLook)
            {
                repositoryItemLook.ValueMember = "Key";
                repositoryItemLook.DisplayMember = "Value";
            }
            else
            if (item is LookUpEdit lookUpEdit)
            {
                lookUpEdit.Properties.ValueMember = "Key";
                lookUpEdit.Properties.DisplayMember = "Value";
            }
            else
            if (item is Control control)
            {
                foreach (object ctrl in control.Controls)
                {
                    SetupLookups(ctrl);
                }
            }
        }
        
        public static string CalculateProcessing(int processed, int total)
        {
            return string.Format("{0} из {1}", processed, total);
        }

        public static string CalculateRemaining(DateTime processStarted, int totalElements, int processedElements)
        {
            int secondsRemaining = 0;
            int totalSecond = (int)(DateTime.Now - processStarted).TotalSeconds;

            if (totalSecond > 0)
            {
                int itemsPerSecond = processedElements / totalSecond;

                if (itemsPerSecond > 0)
                    secondsRemaining = (totalElements - processedElements) / itemsPerSecond;
            }

            return new TimeSpan(0, 0, secondsRemaining).ToString(@"hh\:mm\:ss");
        }

        public static string CalculateDuration(DateTime processStarted)
        {
            return TimeSpan.FromTicks(DateTime.Now.Subtract(processStarted).Ticks).ToString(@"hh\:mm\:ss");
        }

        public static bool EmailIsValid(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);

            if (isMatch.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        //public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //[DllImport("USER32.DLL")]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);

        //[DllImport("kernel32.dll", SetLastError = true)]
        //public static extern bool SetDllDirectory(string lpPathName);

        //public static async void WaitAndKeysendWindowAsinc(string win_title, string sent_text)
        //{
        //    int wait_step = 0;
        //    while (wait_step < 1000)
        //    {
        //        await Task.Delay(50);
        //        IntPtr DialogHandle = FindWindow(null, win_title);
        //        if (DialogHandle != IntPtr.Zero)
        //        {
        //            SetForegroundWindow(DialogHandle);
        //            if (sent_text == "")
        //                SendKeys.Send("{ENTER}");
        //            else
        //            {
        //                await Task.Delay(100);
        //                SendKeys.Send(sent_text + "{ENTER}");
        //            }
        //            break;
        //        }
        //        wait_step++;
        //    }
        //}

        private static string CountSumm(string barcode)
        {
            if (barcode.Length < 12)
                return "";

            barcode = barcode.Substring(0, 12);
            int cnt = 0;
            int sum = 0;
            for (int j = barcode.Length - 1; j > -1; j--)
            {
                cnt += 1;
                if (cnt % 2 == 0)
                {
                    sum += int.Parse(barcode[j].ToString());
                }
                else
                {
                    sum += int.Parse(barcode[j].ToString()) * 3;
                }
            }
            sum = (10 - (sum % 10)) % 10;

            return sum.ToString();

        }

        private static bool Ean13(string barcode)
        {
            if (barcode.Substring(barcode.Length - 1, 1) == CountSumm(barcode))
                return true;
            return false;
        }

        public static string CheckBarcode(string barcode)
        {
            string result = barcode.Trim();
            bool isBar = Regex.IsMatch(result, @"\d");
            if (isBar && result.Length < 13 && result.Length > 10)
            {
                while (result.Length < 13)
                    result = "0" + result;

                if (!Ean13(result))
                {
                    result = barcode.Trim();
                    while (result.Length < 10)
                        result = "0" + result;
                    result += CountSumm(result);
                }

                if (result.Length < 13)
                    result = barcode.Trim();
            }
            else if (result.Length > 13)
            {
                result = null;
            }

            return result;
        }

        //public static void SaveTextToFile(string text, string filePath, bool openDir = false)
        //{
        //    string dir = AppConfig.OutputDir;
        //    filePath = Path.Combine(dir, Path.GetFileNameWithoutExtension(filePath) + ".txt");

        //    if (!Directory.Exists(dir))
        //        Directory.CreateDirectory(dir);

        //    File.WriteAllText(filePath, text, Encoding.GetEncoding(1251));

        //    if (!File.Exists(filePath) || !openDir)
        //    {
        //        return;
        //    }

        //    Process PrFolder = new Process();
        //    ProcessStartInfo psi = new ProcessStartInfo();
        //    psi.CreateNoWindow = false;
        //    psi.WindowStyle = ProcessWindowStyle.Normal;
        //    psi.FileName = "explorer";
        //    psi.Arguments = @"/n, /select, """ + filePath + @"""";
        //    PrFolder.StartInfo = psi;
        //    PrFolder.Start();
        //}

        private static string pass = "AbCdEfGh";

        public static string DeCrypt(string filePath)
        {
            string result = null;
            if (File.Exists(filePath))
                try
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider())
                        {
                            cryptic.Key = ASCIIEncoding.ASCII.GetBytes(pass);
                            cryptic.IV = ASCIIEncoding.ASCII.GetBytes(pass);
                            using (CryptoStream crStream = new CryptoStream(stream, cryptic.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                using (StreamReader reader = new StreamReader(crStream))
                                {
                                    result = reader.ReadToEnd();
                                    reader.Close();
                                }
                            }
                        }
                        stream.Close();
                    }
                }
                catch { }
            return result;
        }

        public static void EnCrypt(string filePath, string content)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {

                using (DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider())
                {

                    cryptic.Key = ASCIIEncoding.ASCII.GetBytes(pass);
                    cryptic.IV = ASCIIEncoding.ASCII.GetBytes(pass);

                    using (CryptoStream crStream = new CryptoStream(stream, cryptic.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] data = ASCIIEncoding.ASCII.GetBytes(content);

                        crStream.Write(data, 0, data.Length);

                        crStream.Close();
                    }
                }
                stream.Close();
            }
        }

    }
}