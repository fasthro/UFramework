/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Launch
{
    public partial class Slider1 : GSlider
    {
        public GGraph n0;
        public GGraph bar;
        public Slider1_grip grip;
        public const string URL = "ui://9fwlj8d0jgzfe";

        public static Slider1 CreateInstance()
        {
            return (Slider1)UIPackage.CreateObject("Launch", "Slider1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            n0 = (GGraph)GetChildAt(0);
            bar = (GGraph)GetChildAt(1);
            grip = (Slider1_grip)GetChildAt(2);
        }
    }
}