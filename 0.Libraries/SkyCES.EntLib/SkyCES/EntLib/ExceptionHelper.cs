namespace SkyCES.EntLib
{
    using System;
    using System.Configuration;

    public class ExceptionHelper
    {
        public static void ProcessException(Exception ex)
        {
            ProcessException(ex, true);
        }

        public static void ProcessException(Exception ex, bool log)
        {
            if (log)
            {
                RecordLog(ex);
            }
            ShowMessage(ex.Message);
        }

        public static void ProcessExceptionForWebService(Exception ex)
        {
            new TxtLog(ServerHelper.MapPath(@"\LogWebService\")).Write(ex.ToString());
        }

        private static void RecordLog(Exception ex)
        {
            new TxtLog(ServerHelper.MapPath(@"\Log\")).Write(ex.ToString());
        }

        private static void ShowMessage(string message)
        {
            IMessage message2 = null;
            switch (((MessageType)Enum.Parse(typeof(MessageType), ConfigurationManager.AppSettings["MessageType"])))
            {
                case MessageType.WebType:
                    message2 = new WebMessage();
                    break;

                case MessageType.WinType:
                    message2 = new WinMessage();
                    break;
            }
            if (message2 != null)
            {
                message2.Show(message);
            }
        }
    }
}