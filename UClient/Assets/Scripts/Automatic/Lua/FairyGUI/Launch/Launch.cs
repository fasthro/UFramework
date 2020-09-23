/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Launch
{
    public partial class Launch : GComponent
    {
        public Test _n0;
        public GImage _n1;
        public Button1 _n3;
        public ProgressBar1 _n4;
        public GTextField _n5;
        public GRichTextField _n6;
        public GTextInput _n7;
        public GGraph _n8;
        public GList _n9;
        public GLoader _n10;
        public GLoader3D _n11;
        public ComboBox1 n16;
        public GMovieClip _n13;
        public Slider1 _n14;
        public Label1 _n15;
        public GGroup _n17;
        public const string URL = "ui://9fwlj8d0gmfc0";

        public static Launch CreateInstance()
        {
            return (Launch)UIPackage.CreateObject("Launch", "Launch");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            _n0 = (Test)GetChildAt(0);
            _n1 = (GImage)GetChildAt(1);
            _n3 = (Button1)GetChildAt(2);
            _n4 = (ProgressBar1)GetChildAt(3);
            _n5 = (GTextField)GetChildAt(4);
            _n6 = (GRichTextField)GetChildAt(5);
            _n7 = (GTextInput)GetChildAt(6);
            _n8 = (GGraph)GetChildAt(7);
            _n9 = (GList)GetChildAt(8);
            _n10 = (GLoader)GetChildAt(9);
            _n11 = (GLoader3D)GetChildAt(10);
            n16 = (ComboBox1)GetChildAt(11);
            _n13 = (GMovieClip)GetChildAt(12);
            _n14 = (Slider1)GetChildAt(13);
            _n15 = (Label1)GetChildAt(14);
            _n17 = (GGroup)GetChildAt(15);
        }
    }
}