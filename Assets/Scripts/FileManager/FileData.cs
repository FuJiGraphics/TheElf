
using System.Collections.Generic;

public class FileData
{
    public string Path { get; private set; }
    public Dictionary<string, dynamic> Data { get; private set; }

    public bool GetBool(string key)
        => (bool)Data[key];
    public int GetInt(string key)
        => (int)Data[key];
    public float GetFloat(string key)
        => (float)Data[key];
    public string GetString(string key)
        => (string)Data[key];

    public bool SetBool(string key, bool value)
        => Data[key] = value;
    public int SetInt(string key, int value)
        => Data[key] = value;
    public float SetFloat(string key, float value)
        => Data[key] = value;
    public string SetString(string key, string value)
        => Data[key] = value;
}