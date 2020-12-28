namespace UFramework.Lockstep
{
    public class BaseView
    {
        public ViewObject viewObj { get; private set; }

        public void BindViewObject(ViewObject viewObj)
        {
            this.viewObj = viewObj;
        }
    }
}