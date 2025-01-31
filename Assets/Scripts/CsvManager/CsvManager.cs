
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

public static class CsvManager
{
    public static IEnumerable<T> Load<T>(string csvPath)
    {
        if (!File.Exists(csvPath))
            return null;

        using (var reader = new StreamReader(csvPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList();
        }
    }

    public static List<T> ToList<T>(string keys)
        where T : IConvertible
    {
        List<T> result = null;
        if (!string.IsNullOrEmpty(keys))
        {
            result = new List<T>();
            var keyList = keys.Split(',');
            for (int i = 0; i < keyList.Length; ++i)
            {
                result.Add((T)Convert.ChangeType(keyList[i], typeof(T)));
            }
        }
        return result;
    }
}