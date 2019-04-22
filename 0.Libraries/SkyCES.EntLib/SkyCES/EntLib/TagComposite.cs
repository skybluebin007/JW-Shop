namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Generic;

    public class TagComposite : BaseTag
    {
        private List<BaseTag> baseTagList = new List<BaseTag>();

        public void AddTag(BaseTag baseTag)
        {
            this.baseTagList.Add(baseTag);
        }

        public override void TagHandler(ref string content)
        {
            foreach (BaseTag tag in this.baseTagList)
            {
                tag.TagHandler(ref content);
            }
        }
    }
}

