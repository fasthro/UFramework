/*
 * @Author: fasthro
 * @Date: 2020-01-04 14:41:16
 * @Description: 国际化对象
 */
namespace UFramework.Language
{
    public class i18nObject
    {
        public int model { get; private set; }
        public int key { get; private set; }

        public i18nObject(int model, int key)
        {
            this.model = model;
            this.key = key;
        }

        // TODO
        // public override string ToString() { return i18n.Get(model, key); }
    }
}