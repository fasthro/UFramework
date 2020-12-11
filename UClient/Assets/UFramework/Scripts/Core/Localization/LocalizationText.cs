/*
 * @Author: fasthro
 * @Date: 2020-01-04 14:41:16
 * @Description: 语言Item
 */
namespace UFramework.Core
{
    public class LocalizationText
    {
        public int model { get; private set; }
        public int key { get; private set; }

        public LocalizationText(int model, int key)
        {
            this.model = model;
            this.key = key;
        }

        public override string ToString() { return Localization.GetText(model, key); }
    }
}