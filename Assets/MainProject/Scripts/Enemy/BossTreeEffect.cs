using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTreeEffect : MonoBehaviour
{
    public float curHealth = 100.0f;

    public GameObject treeObj = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            GameManager.Inst.MainPlayer.enemyHitClip.Play();
            Sword weapon = other.GetComponent<Sword>();
            curHealth -= weapon.damage;

            // 나무를 파괴 시 
            if(curHealth <= 0)
            {
                BossTree tree = FindObjectOfType<BossTree>();
                // 부모 나무의 isEffectOn을 true로 바꿔 BossEnemy에서 더이상 이펙트를 찾지 않음
                tree.isEffectOn = true;
                // 부모 나무의 isDestroy를 true로 바꿔 BossEnemy에서 패턴 파훼 실행
                tree.isDestroy = true;
                Destroy(treeObj);
            }
        }
    }
}
