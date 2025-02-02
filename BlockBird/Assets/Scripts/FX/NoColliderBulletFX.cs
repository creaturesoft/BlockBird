using System.Collections;
using UnityEngine;

public class NoColliderBulletFX : MonoBehaviour
{

    protected float effectDuration = 5.0f;  // ����Ʈ�� ������ ���� �ð�
    virtual public void init(float size = 1f)
    {
        transform.localScale = new Vector3(size, size, 1);
        Destroy(gameObject, effectDuration);
    }


}