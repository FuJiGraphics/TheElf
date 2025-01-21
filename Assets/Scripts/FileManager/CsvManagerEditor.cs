using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CsvManager))]
public class CsvManagerEditor : Editor
{
    protected string fileLoadLabel = "File Path";
    protected List<string> filters = new List<string>();
    protected CsvManager fileManager;

    private StringBuilder m_Filters = new StringBuilder();

    public override void OnInspectorGUI()
    {
        fileManager = (CsvManager)target;

        this.DrawLoadFileGUI();
        this.DrawFiltersGUI();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(fileManager);
        }
    }

    private void DrawLoadFileGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fileLoadLabel, GUILayout.Width(70.0f));
        if (GUILayout.Button("Open..."))
        {
            string open = EditorUtility.OpenFilePanel("", "", "txt");
            if (open != null && File.Exists(open))
            {
                fileManager.OnLoad(open);
            }
            GUIUtility.ExitGUI();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawFiltersGUI()
    {
        if (filters.Count == 0)
        {
            filters.Add("txt");
        }

        for (int i = 0; i < filters.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Filters[{i}]", GUILayout.Width(70.0f));

            string temp = "";
            temp = EditorGUILayout.TextField(filters[i]);
            if (temp != filters[i])
            {
                if (temp.Contains(","))
                    Debug.LogError($"추가하려는 filter 포맷이 잘못되었습니다 {temp}");
                else
                {
                    filters[i] = temp;
                    this.UpdateFilters();
                }
            }

            if (GUILayout.Button("+", GUILayout.Width(30.0f)))
            {
                filters.Add("txt");
                this.UpdateFilters();
            }

            if (filters.Count > 0 && GUILayout.Button("-", GUILayout.Width(30.0f)))
            {
                filters.RemoveAt(filters.Count - 1);
                this.UpdateFilters();
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void UpdateFilters()
    {
        m_Filters.Clear();
        foreach (string filter in filters)
        {
            if (filter == filters[filters.Count - 1])
                m_Filters.Append(filter);
            else
                m_Filters.Append(filter + ",");
        }
    }
} // class FileManagerEditor
