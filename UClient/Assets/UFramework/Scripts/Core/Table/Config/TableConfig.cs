/*
 * @Author: fasthro
 * @Date: 2020-05-30 17:18:34
 * @Description: table config
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEngine;

namespace UFramework.Table
{
    public class TableConfig : IConfigObject
    {
        public string name { get { return "TableConfig"; } }

        public FormatOptions outFormatOptions = FormatOptions.CSV;
        public string tableModelNamespace;
        public Dictionary<string, DataFormatOptions> tableDictionary = new Dictionary<string, DataFormatOptions>();

        public void Save()
        {
            UConfig.Write<TableConfig>(this);
        }
    }
}
