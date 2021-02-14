using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Core
{
    public class ExcelReader
    {
        Dictionary<string, List<List<string>>> excel_records = new Dictionary<string, List<List<string>>>();

        public static ExcelReader LoadFromExcel(Stream stream)
        {
            ExcelReader This = new ExcelReader();
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx)
            using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
            {
                // Choose one of either 1 or 2:

                // 1. Use the reader methods
                do
                {
                    string name = reader.Name;

                    List<List<string>> records = new List<List<string>>();

                    while (reader.Read())
                    {
                        List<string> record = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object v = reader.GetValue(i);
                            record.Add(v != null ? v.ToString() : "");
                        }
                        records.Add(record);
                    }

                    This.excel_records.Add(name, records);

                } while (reader.NextResult());

                // 2. Use the AsDataSet extension method
                //var result = reader.

                // The result of each spreadsheet is in result.Tables
            }

            return This;
        }

        public static ExcelReader LoadFromExcel(string file_pathname)
        {
            using (var stream = File.Open(file_pathname, FileMode.Open, FileAccess.Read))
            {
                return LoadFromExcel(stream);
            }
        }

        static List<T> ReadList_Internal<T>(List<List<string>> records, int data_start_line, out string key) where T : new()
        {
            List<T> table = new List<T>();

            //第一行是标题，第二行是说明，第三行是类型
            List<string> titles = records[0];

            key = titles[0];

            for (int i = data_start_line; i < records.Count; i++)
            {
                List<string> cells = records[i];

                if (cells.Count != titles.Count) //无效的单元
                {
                    //Debug.LogWarning(path + ":Invalid Table Line " + i);
                    continue;
                }


                T t = new T();

                for (int j = 0; j < titles.Count; j++)
                {
                    FieldInfo fi = t.GetType().GetField(titles[j]);
                    if (fi != null)
                    {
                        if (!string.IsNullOrEmpty(cells[j]))
                        {
                            try
                            {
                                if (fi.FieldType.IsPrimitive || fi.FieldType == typeof(string) || fi.FieldType.IsEnum)
                                {
                                    object v = System.ComponentModel.TypeDescriptor.GetConverter(fi.FieldType).ConvertFrom(cells[j]);
                                    fi.SetValue(t, v);
                                }
                                else
                                {
                                    object v = Core.JSON.ToObject(cells[j], fi.FieldType);
                                    fi.SetValue(t, v);
                                }

                            }
                            catch (Exception e)
                            {
                                Core.Logger.LogError(e.Message);
                                Core.Logger.LogError(cells[j] + "parse error:" + e.StackTrace);
                            }

                        }
                        else
                        {
                            if (fi.FieldType == typeof(string))
                            {
                                if (fi.GetValue(t) == null)
                                    fi.SetValue(t, "");
                            }
                        }
                    }
                    else
                    {
                        PropertyInfo pi = t.GetType().GetProperty(titles[j]);
                        if (pi != null)
                        {
                            if (!string.IsNullOrEmpty(cells[j]))
                            {
                                try
                                {
                                    if (pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string) || pi.PropertyType.IsEnum)
                                    {
                                        object v = System.ComponentModel.TypeDescriptor.GetConverter(pi.PropertyType).ConvertFrom(cells[j]);
                                        pi.SetValue(t, v);
                                    }
                                    else
                                    {
                                        object v = Core.JSON.ToObject(cells[j], pi.PropertyType);
                                        pi.SetValue(t, v);
                                    }

                                }
                                catch (Exception e)
                                {
                                    Core.Logger.LogError(e.Message);
                                    Core.Logger.LogError(cells[j] + "parse error:" + e.StackTrace);
                                }

                            }
                            else
                            {
                                if (pi.PropertyType == typeof(string))
                                {
                                    if (pi.GetValue(t) == null)
                                        pi.SetValue(t, "");
                                }
                            }
                        }
                    }
                }
                table.Add(t);
            }
            return table;
        }

        static List<T> ReadList<T>(List<List<string>> records, int data_start_line) where T : new()
        {
            string key = null;
            return ReadList_Internal<T>(records, data_start_line, out key);
        }

        static Dictionary<TKey, TValue> ReadCSVDic<TKey, TValue>(List<List<string>> records, int data_start_line = 3) where TValue : new()
        {
            Dictionary<TKey, TValue> table = new Dictionary<TKey, TValue>();
            string key = null;
            List<TValue> list = ReadList_Internal<TValue>(records, data_start_line, out key);

            FieldInfo KeyField = typeof(TValue).GetField(key);
            if(KeyField!=null)
            {
                foreach (var it in list)
                {
                    table.Add((TKey)KeyField.GetValue(it), it);
                }
            }
            else
            {
                PropertyInfo property = typeof(TValue).GetProperty(key);
                if (property != null)
                {
                    foreach (var it in list)
                    {
                        table.Add((TKey)property.GetValue(it), it);
                    }
                }
            }



            return table;
        }

        public List<T> LoadDataList<T>(string fileName) where T : new()
        {
            return ReadList<T>(excel_records[fileName], 3);
        }
        public Dictionary<K, T> LoadDataDic<K, T>(string fileName) where T : new()
        {
            return ReadCSVDic<K, T>(excel_records[fileName], 3);
        }
    }
}
