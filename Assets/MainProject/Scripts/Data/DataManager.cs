using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public string name;
    public float level = 1;
    public float HP = 100.0f;
    public float MaxHP = 100.0f;
    public float EXP = 0.0f;
    public float MaxEXP = 100.0f;
    public Vector3 playerPos = new Vector3(-220, 0, -230);
}

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null;

    public static DataManager Inst
    {
        get
        {
            return instance;
        }
    }

    public PlayerData Player = new PlayerData();

    public string path;
    public int nowSlot;

    private void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion

        path = Application.persistentDataPath + "/save";
    }

    private void Start()
    {
    }

    public void SaveData()
    {
        Debug.Log("세이브");
        string data = JsonUtility.ToJson(Player);
        File.WriteAllText(path + nowSlot.ToString(), data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString());
        Player = JsonUtility.FromJson<PlayerData>(data);
    }

    public void DataClear()
    {
        nowSlot = -1;
        Player = new PlayerData();
    }
}
