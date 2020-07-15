/*
 * @Author: fasthro
 * @Date: 2020-01-04 14:41:16
 * @Description: 语言Item
 */
namespace UFramework.Localization
{
    public class LanguageItem
    {
        public int model { get; private set; }
        public int key { get; private set; }

        public LanguageItem(int model, int key)
        {
            this.model = model;
            this.key = key;
        }

        public override string ToString() { return Language.Get(model, key); }
    }
}