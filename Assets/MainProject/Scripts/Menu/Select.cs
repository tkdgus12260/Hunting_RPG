using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Select : MonoBehaviour
{
    public GameObject selectUI;
    public GameObject creat;
    public Text[] slotText;
    public Text newPlayerName;

    private bool[] saveFile = new bool[3];

    private void Start()
    {
        // 슬롯 별 저장된 데이터가 존재하는지 확인 
        for (int i = 0; i < 3; i++) {
            // 데이터가 존재하면 저장 된 데이터 확인 후 저장 된 이름 출력
            if (File.Exists(DataManager.Inst.path +$"{i}"))
            {
                saveFile[i] = true;
                DataManager.Inst.nowSlot = i;
                DataManager.Inst.LoadData();
                slotText[i].text = DataManager.Inst.Player.name;
            }
            // 데이터가 없을 시 비어있음 텍스트 출력
            else
            {
                slotText[i].text = "비어있음";
            }
        }
        // 불러온 값 리셋 
        DataManager.Inst.DataClear();
    }

    public void Slot(int number)
    {
        DataManager.Inst.nowSlot = number;

        // 저장된 데이터가 있을 때 데이터를 불러온 뒤 게임 실행
        if (saveFile[number])
        {
            DataManager.Inst.LoadData();
            PlayGame();
        }
        // 저장된 데이터가 없을 때 생성 창 on
        else
        {
            Creat();
        }

    }

    public void SelectOn()
    {
        selectUI.gameObject.SetActive(true);
    }

    public void SelectOff()
    {
        selectUI.gameObject.SetActive(false);
    }

    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }

    public void CreatOff()
    {
        creat.gameObject.SetActive(false);
    }

    // 게임 씬으로 이동
    public void PlayGame()
    {
        // 데이터가 없을 때 새로 데이터를 만들어 줌
        if (!saveFile[DataManager.Inst.nowSlot])
        {
            DataManager.Inst.Player.name = newPlayerName.text;
            DataManager.Inst.SaveData();
        }

        SceneManager.LoadScene(1);
    }
}
