namespace SkyCES.EntLib
{
    using Microsoft.Office.Interop.Word;
    using System;
    using System.Reflection;

    public class WordHelper
    {
        private ApplicationClass applicationClass = new ApplicationClass();
        private Document document;
        private object missing = Missing.Value;

        public void ClearStyle()
        {
            this.applicationClass.Selection.Font.Bold=0;
            this.applicationClass.Selection.Font.Italic=(0);
            this.applicationClass.Selection.Font.Subscript=(0);
        }

        public void GotoBookMark(string strBookMarkName)
        {
            object obj2 = -1;
            object obj3 = strBookMarkName;
            this.applicationClass.Selection.GoTo(ref obj2, ref this.missing, ref this.missing, ref obj3);
        }

        public void GoToDownCell()
        {
            object obj2 = (WdUnits) 5;
            this.applicationClass.Selection.MoveDown(ref obj2, ref this.missing, ref this.missing);
        }

        public void GoToLeftCell()
        {
            object obj2 = (WdUnits) 12;
            this.applicationClass.Selection.MoveLeft(ref obj2, ref this.missing, ref this.missing);
        }

        public void GoToRightCell()
        {
            object obj2 = (WdUnits) 12;
            this.applicationClass.Selection.MoveRight(ref obj2, ref this.missing, ref this.missing);
        }

        public void GoToTheBeginning()
        {
            object obj2 = (WdUnits) 6;
            this.applicationClass.Selection.HomeKey(ref obj2, ref this.missing);
        }

        public void GoToTheEnd()
        {
            object obj2 = (WdUnits) 6;
            this.applicationClass.Selection.EndKey(ref obj2, ref this.missing);
        }

        public void GoToTheTable(int ntable)
        {
            object obj2 = (WdUnits) 15;
            object obj3 = (WdGoToDirection) 1;
            object obj4 = 1;
            this.applicationClass.Selection.GoTo(ref obj2, ref obj3, ref obj4, ref this.missing);
            this.applicationClass.Selection.Find.ClearFormatting();
            this.applicationClass.Selection.Text=("");
        }

        public void GoToUpCell()
        {
            object obj2 = (WdUnits) 5;
            this.applicationClass.Selection.MoveUp(ref obj2, ref this.missing, ref this.missing);
        }

        public void InsertLineBreak()
        {
            this.applicationClass.Selection.TypeParagraph();
        }

        public void InsertLineBreak(int nline)
        {
            for (int i = 0; i < nline; i++)
            {
                this.applicationClass.Selection.TypeParagraph();
            }
        }

        public void InsertPagebreak()
        {
            object obj2 = 7;
            this.applicationClass.Selection.InsertBreak(ref obj2);
        }

        public void InsertText(string strText)
        {
            this.applicationClass.Selection.TypeText(strText);
        }

        public void New()
        {
            object obj2 = Missing.Value;
            this.document = this.applicationClass.Documents.Add(ref obj2, ref obj2, ref obj2, ref obj2);
            this.document.Activate();
        }

        public void Open(string strFileName)
        {
            object obj2 = strFileName;
            object obj3 = false;
            object obj4 = true;
            this.document = this.applicationClass.Documents.Open(ref obj2, ref this.missing, ref obj3, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref obj4, ref this.missing, ref this.missing, ref this.missing, ref this.missing);
            this.document.Activate();
        }

        public void Quit()
        {
            object obj2 = Missing.Value;
            this.applicationClass.Application.Quit(ref obj2, ref obj2, ref obj2);
        }

        public bool ReplaceText(string findStr, string replaceStr)
        {
            object obj2 = (WdReplace) 2;
            this.applicationClass.Selection.Find.ClearFormatting();
            object obj3 = findStr;
            this.applicationClass.Selection.Find.Replacement.ClearFormatting();
            this.applicationClass.Selection.Find.Replacement.Text=(replaceStr);
            return this.applicationClass.Selection.Find.Execute(ref obj3, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref obj2, ref this.missing, ref this.missing, ref this.missing, ref this.missing);
        }

        public void Save()
        {
            this.document.Save();
        }

        public void SaveAs(string strFileName)
        {
            object obj2 = strFileName;
            this.document.SaveAs(ref obj2, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing);
        }

        public void SaveAsHtml(string strFileName)
        {
            object obj2 = strFileName;
            object obj3 = 8;
            this.document.SaveAs(ref obj2, ref obj3, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing, ref this.missing);
        }

        public void SetAlignment(string strType)
        {
            string str = strType;
            if (str != null)
            {
                if (!(str == "Center"))
                {
                    if (str == "Left")
                    {
                        this.applicationClass.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    }
                    else if (str == "Right")
                    {
                        this.applicationClass.Selection.ParagraphFormat.Alignment= WdParagraphAlignment.wdAlignParagraphRight;
                    }
                    else if (str == "Justify")
                    {
                        this.applicationClass.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                    }
                }
                else
                {
                    this.applicationClass.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                }
            }
        }

        public void SetFont(string strType)
        {
            string str = strType;
            if (str != null)
            {
                if (!(str == "Bold"))
                {
                    if (str == "Italic")
                    {
                        this.applicationClass.Selection.Font.Italic=(1);
                    }
                    else if (str == "Underlined")
                    {
                        this.applicationClass.Selection.Font.Subscript=(0);
                    }
                }
                else
                {
                    this.applicationClass.Selection.Font.Bold=(1);
                }
            }
        }

        public void SetFontColor(WdColor color)
        {
            this.applicationClass.Selection.Font.Color=(color);
        }

        public void SetFontName(string strType)
        {
            this.applicationClass.Selection.Font.Name=(strType);
        }

        public void SetFontSize(int nSize)
        {
            this.applicationClass.Selection.Font.Size=((float) nSize);
        }
    }
}

