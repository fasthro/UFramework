/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Launch
{
    public partial class ComboBox1_item : GButton
    {
        public Controller button;
        public GGraph n0;
        public GGraph n1;
        public GTextField title;
        public const string URL = "ui://9fwlj8d0jgzfa";

        public static ComboBox1_item CreateInstance()
        {
            return (ComboBox1_item)UIPackage.CreateObject("Launch", "ComboBox1_item");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            button = GetControllerAt(0);
            n0 = (GGraph)GetChildAt(0);
            n1 = (GGraph)GetChildAt(1);
            title = (GTextField)GetChildAt(2);
        }
    }
}