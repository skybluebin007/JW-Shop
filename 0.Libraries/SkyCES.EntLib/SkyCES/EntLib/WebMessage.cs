namespace SkyCES.EntLib
{
    using System;

    public class WebMessage : IMessage
    {
        public void Show(string message)
        {
            ScriptHelper.AlertFront(message);
        }
    }
}

