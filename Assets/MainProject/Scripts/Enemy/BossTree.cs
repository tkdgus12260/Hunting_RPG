using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTree : MonoBehaviour
{
    public GameObject treeEffect = null;
    // SetActive(false)의 TreeEffect를 이중 접근
    public bool isEffectOn = false;
    public bool isDestroy = false;

    // 패턴 파훼 실패 시 실행 될 함수
    public void FailPattern()
    {
        Destroy(gameObject);
    }
}
