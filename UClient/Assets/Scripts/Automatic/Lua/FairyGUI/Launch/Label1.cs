/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Launch
{
    public partial class Label1 : GLabel
    {
        public GTextField title;
        public const string URL = "ui://9fwlj8d0jgzf9";

        public static Label1 CreateInstance()
        {
            return (Label1)UIPackage.CreateObject("Launch", "Label1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            title = (GTextField)GetChildAt(0);
        }
    }
}