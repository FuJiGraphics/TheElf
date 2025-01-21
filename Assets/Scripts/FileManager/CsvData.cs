using System.Collections.Generic;

public class CsvData
{
    public string Path { get; private set; }
    public Dictionary<string, object> Data { get; private set; }

    public CsvData(string path)
    {
        Path = path;
        Data = new Dictionary<string, object>();
    }

    public bool GetBool(string key) => Data.ContainsKey(key) && Data[key] is bool value ? value : false;
    public int GetInt(string key) => Data.ContainsKey(key) && Data[key] is int value ? value : 0;
    public float GetFloat(string key) => Data.ContainsKey(key) && Data[key] is float value ? value : 0f;
    public string GetString(string key) => Data.ContainsKey(key) && Data[key] is string value ? value : string.Empty;

    public void SetBool(string key, bool value) => Data[key] = value;
    public void SetInt(string key, int value) => Data[key] = value;
    public void SetFloat(string key, float value) => Data[key] = value;
    public void SetString(string key, string value) => Data[key] = value;
}
