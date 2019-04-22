namespace SkyCES.EntLib
{
    using System;

    public abstract class BaseTag
    {
        protected BaseTag()
        {
        }

        public abstract void TagHandler(ref string content);
    }
}

