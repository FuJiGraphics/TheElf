using System.Collections.Generic;
using UnityEngine;

public abstract class FileManager : MonoBehaviour
{
    abstract public Dictionary<string, FileData> Files { get; set; }
    abstract public void OnLoad(string path);
} // class FileManager