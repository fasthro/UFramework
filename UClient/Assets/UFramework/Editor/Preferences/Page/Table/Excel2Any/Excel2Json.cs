// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    public class Excel2Json : Excel2Any
    {
        public Excel2Json(ExcelReader reader) : base(reader)
        {
            var dataList = new List<Dictionary<string, object>>();
            for (var i = 0; i < reader.rows.Count; i++)
            {
                var data = new Dictionary<string, object>();
                for (int k = 0; k < reader.fields.Count; k++)
                {
                    data.Add(reader.fields[k], reader.rows[i].datas[k]);
                }

                dataList.Add(data);
            }

            IOPath.FileCreateText(reader.options.dataOutFilePath, JsonUtility.ToJson(dataList));
        }
    }
}