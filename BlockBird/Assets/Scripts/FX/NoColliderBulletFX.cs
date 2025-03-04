using System.Collections;
using UnityEngine;

public class NoColliderBulletFX : MonoBehaviour
{

    protected float effectDuration = 5.0f;  // ����Ʈ�� ������ ���� �ð�


    public AudioSource defaultAudio;

    public virtual void init(float size = 1f)
    {
        if (Time.time - GameManager.effectSoundLastPlayTime >= GameManager.effectSoundPlayCooldown && defaultAudio != null && SoundManager.Instance.isSFXOn)
        {
            defaultAudio?.PlayOneShot(defaultAudio.clip);
            GameManager.effectSoundLastPlayTime = Time.time;
        }

        transform.localScale = new Vector3(size, size, 1);
        Destroy(gameObject, effectDuration);
    }


}