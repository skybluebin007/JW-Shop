namespace SkyCES.EntLib
{
    using System;

    public abstract class LogBase
    {
        protected LogBase()
        {
        }

        public abstract void Write(string message);
    }
}

