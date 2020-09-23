/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Launch
{
    public partial class ComboBox1 : GComboBox
    {
        public Controller button;
        public GGraph n0;
        public GGraph n1;
        public GGraph n2;
        public GTextField title;
        public const string URL = "ui://9fwlj8d0jgzfc";

        public static ComboBox1 CreateInstance()
        {
            return (ComboBox1)UIPackage.CreateObject("Launch", "ComboBox1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            button = GetControllerAt(0);
            n0 = (GGraph)GetChildAt(0);
            n1 = (GGraph)GetChildAt(1);
            n2 = (GGraph)GetChildAt(2);
            title = (GTextField)GetChildAt(3);
        }
    }
}