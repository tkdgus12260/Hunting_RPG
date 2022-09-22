using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(HitEffectFalse());
    }

    IEnumerator HitEffectFalse()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
