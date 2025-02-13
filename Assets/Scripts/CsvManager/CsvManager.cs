
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class CsvManager
{
    public static IEnumerable<T> Load<T>(string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList();
        }
    }

    public static IEnumerable<T> LoadFromText<T>(string csvText)
    {
        using (var reader = new StringReader(csvText))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList();
        }
    }

    public static void SaveFromText<T>(string filePath, params T[] data)
    {
        var records = new List<T>();
        foreach (var item in data)
        {
            records.Add(item);
        }

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }

    public static void SaveInPersistentDataPath<T>(T data, string fileName, bool append = true)
    {
        if (fileName.Contains(".csv") == false)
        {
            fileName += ".csv";
        }
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        bool fileExists = File.Exists(filePath);
        try
        {
            if (!Directory.Exists(Application.persistentDataPath))
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                Debug.Log($"폴더 생성 완료: {Application.persistentDataPath}");
            }

            using (var writer = new StreamWriter(filePath, append, System.Text.Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                if (!fileExists)
                {
                    csv.WriteHeader<T>();
                    csv.NextRecord();
                }
                csv.WriteRecord(data);
                csv.NextRecord();
                Debug.Log($"CSV 저장 완료: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"CSV 저장 중 오류 발생: {ex.Message}");
        }
    }

    public static void RemoveInPersistentDataPath<T>(string fileName)
    {
        if (fileName.Contains(".csv") == false)
        {
            fileName += ".csv";
        }
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.Delete(filePath);
    }

    public static List<T> ToList<T>(string keys)
        where T : IConvertible
    {
        List<T> result = null;
        if (!string.IsNullOrEmpty(keys))
        {
            result = new List<T>();

            List<string> strList = new List<string>();
            List<T> outputList = new List<T>();
            if (CsvManager.IsListKeys<string>(keys, strList))
            {
                foreach (string record in strList)
                {
                    if (CsvManager.IsFormula(record, outputList))
                    {
                        result.AddRange(outputList);
                        outputList.Clear();
                    }
                    else
                    {
                        result.Add((T)Convert.ChangeType(record, typeof(T)));
                    }
                }
            }
            else
            {
                if (CsvManager.IsFormula(keys, outputList))
                {
                    result.AddRange(outputList);
                }
                else
                {
                    result.Add((T)Convert.ChangeType(keys, typeof(T)));
                }
            }
        }
        return result;
    }

    private static bool IsFormula<T>(string keys, List<T> output) 
    {
        bool result = false;

        var keyList = keys.Split("#");
        if (keyList.Length > 1 && output != null)
        {
            int id = int.Parse(keyList[0]);
            // 범위 식
            var rangeFormula = keyList[1].Split("~");
            if (rangeFormula.Length > 1)
            {
                int increase = int.Parse(rangeFormula[0]);
                int maxCount = int.Parse(rangeFormula[1]);
                int size = id + (increase * maxCount);
                for (int i = id + increase; i <= size; i += increase)
                {
                    T value = (T)Convert.ChangeType(i, typeof(T));
                    output.Add(value);
                }
                result = true;
            }
        }
        return result;
    }

    private static bool IsListKeys<T>(string keys, List<T> output)
    {
        bool result = false;
        var keyList = keys.Split('/');
        if (keyList.Length > 0 && keyList[0].Length > 0)
        {
            for (int i = 0; i < keyList.Length; ++i)
            {
                output.Add((T)Convert.ChangeType(keyList[i], typeof(T)));
            }
            result = true;
        }
        return result;
    }

} // static class CsvManager 