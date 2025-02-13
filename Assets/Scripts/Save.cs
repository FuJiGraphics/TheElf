using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    public string id = "-1";
    public float atk = -1f;
    public int clear = -1;
    private Button m_Button;

    private void Start()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SaveData data = new SaveData();

        DataTable<SaveData>.InitFromPersistentData("SaveData.csv");
        var userData = DataTable<SaveData>.At("255");

        id = userData.Id;
        atk = userData.AttackSpeed;
        clear = userData.ClearStage;

        //data.Id = 0;
        //data.AttackSpeed = 2;
        //data.ClearStage = 5;
        //CsvManager.SaveInPersistentDataPath(data, "SaveData.csv");
    }

} // class Save  
