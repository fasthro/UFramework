/*
 * @Author: fasthro
 * @Date: 2020-06-29 11:26:04
 * @Description: Table Page
 */
using System.Collections.Generic;
using System.IO;
using UFramework.Config;
using UFramework.Table;

namespace UFramework.FrameworkWindow
{
    public class TablePage : IPage
    {
        public string menuName { get { return "Table"; } }
        static TableConfig instance;

        public object GetInstance()
        {
            instance = UConfig.Read<TableConfig>();
            return instance;
        }

        public bool IsPage(string name)
        {
            return menuName.Equals(name);
        }

        public void OnRenderBefore()
        {
            bool hasNew = false;
            if (Directory.Exists(App.TableExcelDirectory()))
            {
                var files = Directory.GetFiles(App.TableExcelDirectory(), "*.xlsx", SearchOption.AllDirectories);
                HashSet<string> fileHashSet = new HashSet<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = IOPath.FileName(files[i], false);
                    fileHashSet.Add(fileName);
                    if (!instance.tableDictionary.ContainsKey(fileName))
                    {
                        hasNew = true;
                        instance.tableDictionary.Add(fileName, DataFormatOptions.Array);
                    }
                }

                List<string> removes = new List<string>();
                instance.tableDictionary.ForEach((item) =>
                {
                    if (!fileHashSet.Contains(item.Key))
                    {
                        removes.Add(item.Key);
                    }
                });

                for (int i = 0; i < removes.Count; i++)
                {
                    hasNew = true;
                    instance.tableDictionary.Remove(removes[i]);
                }
            }
            if (hasNew)
            {
                instance.Save();
            }
        }
    }
}