/*
 * @Author: fasthro
 * @Date: 2019-12-19 15:01:37
 * @Description: 字段类型
 */
using System;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Core
{
    public static class TableTypeUtils
    {
        /// <summary>
        /// type string to field type
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static TableFieldType TypeContentToFieldType(string ts)
        {
            if (TableFieldType.Byte.ToString().Equals(ts)) return TableFieldType.Byte;
            else if (TableFieldType.Int.ToString().Equals(ts)) return TableFieldType.Int;
            else if (TableFieldType.Long.ToString().Equals(ts)) return TableFieldType.Long;
            else if (TableFieldType.Float.ToString().Equals(ts)) return TableFieldType.Float;
            else if (TableFieldType.Double.ToString().Equals(ts)) return TableFieldType.Double;
            else if (TableFieldType.String.ToString().Equals(ts)) return TableFieldType.String;
            else if (TableFieldType.Boolean.ToString().Equals(ts)) return TableFieldType.Boolean;
            else if (TableFieldType.Vector2.ToString().Equals(ts)) return TableFieldType.Vector2;
            else if (TableFieldType.Vector3.ToString().Equals(ts)) return TableFieldType.Vector3;
            else if (TableFieldType.Language.ToString().Equals(ts)) return TableFieldType.Language;
            else if (TableFieldType.ArrayByte.ToString().Equals(ts)) return TableFieldType.ArrayByte;
            else if (TableFieldType.ArrayInt.ToString().Equals(ts)) return TableFieldType.ArrayInt;
            else if (TableFieldType.ArrayLong.ToString().Equals(ts)) return TableFieldType.ArrayLong;
            else if (TableFieldType.ArrayFloat.ToString().Equals(ts)) return TableFieldType.ArrayFloat;
            else if (TableFieldType.ArrayDouble.ToString().Equals(ts)) return TableFieldType.ArrayDouble;
            else if (TableFieldType.ArrayString.ToString().Equals(ts)) return TableFieldType.ArrayString;
            else if (TableFieldType.ArrayBoolean.ToString().Equals(ts)) return TableFieldType.ArrayBoolean;
            else if (TableFieldType.ArrayVector2.ToString().Equals(ts)) return TableFieldType.ArrayVector2;
            else if (TableFieldType.ArrayVector3.ToString().Equals(ts)) return TableFieldType.ArrayVector3;
            else if (TableFieldType.ArrayLanguage.ToString().Equals(ts)) return TableFieldType.ArrayLanguage;
            else if (TableFieldType.Ignore.ToString().Equals(ts)) return TableFieldType.Ignore;
            else return TableFieldType.Unknow;
        }

        /// <summary>
        /// file type to c# variable type string
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static string FieldTypeToTypeContent(TableFieldType fileType)
        {
            switch (fileType)
            {
                case TableFieldType.Byte:
                    return "byte";
                case TableFieldType.Int:
                    return "int";
                case TableFieldType.Long:
                    return "long";
                case TableFieldType.Float:
                    return "float";
                case TableFieldType.Double:
                    return "double";
                case TableFieldType.String:
                    return "string";
                case TableFieldType.Language:
                    return "LocalizationText";
                case TableFieldType.Boolean:
                    return "bool";
                case TableFieldType.Vector2:
                    return "Vector2";
                case TableFieldType.Vector3:
                    return "Vector3";
                case TableFieldType.ArrayByte:
                    return "byte[]";
                case TableFieldType.ArrayInt:
                    return "int[]";
                case TableFieldType.ArrayLong:
                    return "long[]";
                case TableFieldType.ArrayFloat:
                    return "float[]";
                case TableFieldType.ArrayDouble:
                    return "double[]";
                case TableFieldType.ArrayString:
                    return "string[]";
                case TableFieldType.ArrayBoolean:
                    return "bool[]";
                case TableFieldType.ArrayVector2:
                    return "Vector2[]";
                case TableFieldType.ArrayVector3:
                    return "Vector3[]";
                case TableFieldType.ArrayLanguage:
                    return "LocalizationText[]";
                default:
                    return "";
            }
        }

        #region  content to c# value
        /// <summary>
        /// content to bool
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool ContentToBooleanValue(string content)
        {
            return content.Equals("0") ? false : true;
        }

        /// <summary>
        /// content to LanguageItem
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static LocalizationText ContentToLanguageItemValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ':' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return new LocalizationText(int.Parse(datas[0]), int.Parse(datas[1]));
        }

        /// <summary>
        /// content to vector2
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Vector2 ContentToVector2Value(string content)
        {
            if (string.IsNullOrEmpty(content)) return Vector2.zero;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return new Vector2(float.Parse(datas[0]), float.Parse(datas[1]));
        }

        /// <summary>
        /// content to vector3
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Vector3 ContentToVector3Value(string content)
        {
            if (string.IsNullOrEmpty(content)) return Vector3.zero;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return new Vector3(float.Parse(datas[0]), float.Parse(datas[1]), float.Parse(datas[2]));
        }

        /// <summary>
        /// content to byte array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] ContentToArrayByteValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            byte[] data = new byte[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = byte.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to int array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int[] ContentToArrayIntValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            int[] data = new int[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = int.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to long array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static long[] ContentToArrayLongValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            long[] data = new long[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = long.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to float array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static float[] ContentToArrayFloatValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            float[] data = new float[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = float.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to double array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static double[] ContentToArrayDoubleValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            double[] data = new double[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = double.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to bool array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool[] ContentToArrayBooleanValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            bool[] data = new bool[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = datas[i].Equals("0") ? false : true;
            }
            return data;
        }

        /// <summary>
        /// content to bool array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string[] ContentToArrayStringValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            return content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// content to vector2 array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Vector2[] ContentToArrayVector2Value(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { '|' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            Vector2[] data = new Vector2[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = ContentToVector2Value(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to vector3 array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Vector3[] ContentToArrayVector3Value(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { '|' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            Vector3[] data = new Vector3[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = ContentToVector3Value(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to LanguageItem array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static LocalizationText[] ContentToArrayLanguageItemValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separatorArray = new char[] { ',' };
            string[] datasArray = content.Split(separatorArray, StringSplitOptions.RemoveEmptyEntries);
            LocalizationText[] result = new LocalizationText[datasArray.Length];
            for (int i = 0; i < datasArray.Length; i++)
            {
                string dataContent = datasArray[i];
                if (string.IsNullOrEmpty(dataContent)) continue;
                char[] separator = new char[] { ':' };
                string[] datas = dataContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                result[i] = new LocalizationText(int.Parse(datas[0]), int.Parse(datas[1]));
            }
            return result;
        }
        #endregion
    }
}