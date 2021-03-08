// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 15:12
// * @Description:
// --------------------------------------------------------------------------------

using System;

namespace UFramework.Consoles
{
    public class InfoEntry
    {
        public string title { get; private set; }

        public object Value
        {
            get
            {
                try
                {
                    return _valueGetter();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public bool isPrivate { get; private set; }

        private Func<object> _valueGetter;


        public static InfoEntry Create(string name, Func<object> getter, bool isPrivate = false)
        {
            return new InfoEntry
            {
                title = name,
                _valueGetter = getter,
                isPrivate = isPrivate
            };
        }

        public static InfoEntry Create(string name, object value, bool isPrivate = false)
        {
            return new InfoEntry
            {
                title = name,
                _valueGetter = () => value,
                isPrivate = isPrivate
            };
        }
    }
}