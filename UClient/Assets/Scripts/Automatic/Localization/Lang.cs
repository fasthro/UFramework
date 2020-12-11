// UFramework Automatic.
using UFramework.Core;

namespace UFramework.Automatic {
	public class Lang
	{
		public static int test = 0;
		public static int table = 1;

		public static int test_key = 0;
		public static int table_key = 0;

        public static string GetTest(int key)
        {
            return Localization.GetText(test, key);
        }

        public static string GetTable(int key)
        {
            return Localization.GetText(table, key);
        }


	}
}
