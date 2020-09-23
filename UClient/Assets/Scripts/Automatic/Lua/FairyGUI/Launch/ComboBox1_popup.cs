/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Launch
{
    public partial class ComboBox1_popup : GComponent
    {
        public GGraph n0;
        public GList list;
        public const string URL = "ui://9fwlj8d0jgzfb";

        public static ComboBox1_popup CreateInstance()
        {
            return (ComboBox1_popup)UIPackage.CreateObject("Launch", "ComboBox1_popup");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            n0 = (GGraph)GetChildAt(0);
            list = (GList)GetChildAt(1);
        }
    }
}