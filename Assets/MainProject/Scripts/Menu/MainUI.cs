using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BTNType
{
    Play,
    Quit
}

public class MainUI : MonoBehaviour
{
    private SoundManager soundManager = null;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        // 실행중인 현재 bgm 끄기
        soundManager.bgmPlayer.Stop();
        // 0번 bgm 실행
        soundManager.PlayBGM(0);
    }
}
