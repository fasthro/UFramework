/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Launch
{
    public partial class ProgressBar1 : GProgressBar
    {
        public GGraph n0;
        public GGraph bar;
        public const string URL = "ui://9fwlj8d0jgzf7";

        public static ProgressBar1 CreateInstance()
        {
            return (ProgressBar1)UIPackage.CreateObject("Launch", "ProgressBar1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            n0 = (GGraph)GetChildAt(0);
            bar = (GGraph)GetChildAt(1);
        }
    }
}