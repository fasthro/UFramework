/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 json
 */
using System.Collections.Generic;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    public class Excel2Json : Excel2Any
    {
        public Excel2Json(ExcelReader reader) : base(reader)
        {
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
            for (int i = 0; i < reader.rows.Count; i++)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
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