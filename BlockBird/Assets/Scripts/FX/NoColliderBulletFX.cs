using System.Collections;
using UnityEngine;

public class NoColliderBulletFX : MonoBehaviour
{

    protected float effectDuration = 5.0f;  // 이펙트가 끝나는 예상 시간
    virtual public void init(float size = 1f)
    {
        transform.localScale = new Vector3(size, size, 1);
        Destroy(gameObject, effectDuration);
    }


}