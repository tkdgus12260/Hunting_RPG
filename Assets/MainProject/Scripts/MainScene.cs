using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public Controller _playerCtrl = null;
    private SoundManager _soundManager = null;

    private void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        // 현재 실행중인 bgm 끄기
        _soundManager.bgmPlayer.Stop();
        // 1번 bgm 실행
        _soundManager.PlayBGM(1);

        // 씬으로 돌아올 때 파괴된 플레이어 오브젝트를 다시 찾기
        GameManager.Inst.Initialize();
        _playerCtrl.RegisterInputAction();

    }

    private void OnDestroy()
    {
        // esc를 누르고 timescale을 0으로 만들고 메뉴로 나가기 때문에 초기화
        Time.timeScale = 1.0f;
        _playerCtrl.UnRegisterInputAction();
    }

}
