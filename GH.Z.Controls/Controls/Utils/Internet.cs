using GH.AppContext;
using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace GH.Utils
{
    public static class Internet
    {
        public static string Error_message => "Нет подключения к Интернету!!!" + "\r\n" + "Проверьте все параметры подкулючения и попробуйте снова.";
        private static DateTime _last_check = DateTime.Now.AddSeconds(-5);
        private static bool checkStatus = false;

        [DllImport("wininet.dll")]
        static extern bool InternetGetConnectedState(ref InternetConnectionState lpdwFlags, int dwReserved);

        [Flags]
        enum InternetConnectionState : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        static object _syncObj = new object();

        public static bool CheckConnectionForDatabase()
        {
            if (RunContext.IsRemoteDataBase)
            {
                if (_last_check > DateTime.Now.AddSeconds(-5))
                {
                    return checkStatus;
                }
                else
                {
                    _last_check = DateTime.Now;
                }

                checkStatus = false;

                try
                {
                    InternetConnectionState flags = InternetConnectionState.INTERNET_CONNECTION_CONFIGURED | 0;
                    checkStatus = InternetGetConnectedState(ref flags, 0);

                    if (checkStatus)
                        checkStatus = PingServer(new string[]
                                            {
                                                RunContext.RemoteServer,
                                            });

                    if (!checkStatus)
                    {
                        DlgHelper.DlgError(Error_message);
                        Logger.Error(new Exception(Error_message));
                    }

                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

                return checkStatus;
            }
            else
                return true;
        }


        /// <summary>
        /// Пингует сервера, при первом получении ответа от любого сервера возвращает true 
        /// </summary>
        /// <param name="serverList">Список серверов</param>
        /// <returns></returns>
        public static bool PingServer(string[] serverList)
        {
            bool haveAnInternetConnection = false;
            Ping ping = new Ping();
            for (int i = 0; i < serverList.Length; i++)
            {
                PingReply pingReply = ping.Send(serverList[i]);
                haveAnInternetConnection = (pingReply.Status == IPStatus.Success);
                if (haveAnInternetConnection)
                    break;
            }

            return haveAnInternetConnection;
        }
    }
}
