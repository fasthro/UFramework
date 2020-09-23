/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Launch
{
    public partial class Slider1_grip : GButton
    {
        public Controller button;
        public GGraph n0;
        public GGraph n1;
        public GGraph n2;
        public const string URL = "ui://9fwlj8d0jgzfd";

        public static Slider1_grip CreateInstance()
        {
            return (Slider1_grip)UIPackage.CreateObject("Launch", "Slider1_grip");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            button = GetControllerAt(0);
            n0 = (GGraph)GetChildAt(0);
            n1 = (GGraph)GetChildAt(1);
            n2 = (GGraph)GetChildAt(2);
        }
    }
}