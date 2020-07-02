// UFramework Auto Generate.
using sys = UFramework.i18n.Language;

public static class Language
{
	public static int test = 0;
	public static int table = 1;

	public static int test_key = 0;
	public static int table_key = 0;

    public static string GetTest(int key)
    {
        return sys.Get(test, key);
    }

    public static string GetTable(int key)
    {
        return sys.Get(table, key);
    }


}
