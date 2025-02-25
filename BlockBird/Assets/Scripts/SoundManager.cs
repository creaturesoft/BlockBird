using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource breakBlockAudio;
    public AudioSource breakBossBlockAudio;

    public AudioSource startAudio;
    public AudioSource getItemAudio;
    public AudioSource successAudio;

    public AudioSource speedUpItemAudio;
    public AudioSource friendsItemAudio;

    void Awake()
    {
        // �̱��� �ν��Ͻ��� �̹� �����ϸ� �ı�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ���� ��ü�� ����
        Instance = this;
    }

    public void PlayBreakBlockAudio(bool isBoss)
    {
        if (isBoss)
        {
            breakBossBlockAudio.Stop();
            breakBossBlockAudio.Play();

        }
        else
        {
            breakBlockAudio.Stop();
            breakBlockAudio.Play();
        }
    }

    public void PlayStartButtonAudio()
    {
        startAudio.Play();
    }

    public void PlayGetItemAudio()
    {
        getItemAudio.Stop();
        getItemAudio.Play();
    }

    public void PlaySuccessAudio()
    {
        if (!successAudio.isPlaying)
        {
            successAudio.Play();
        }
    }

    public void PlaySpeedUpItemAudio()
    {
        speedUpItemAudio.Stop();
        speedUpItemAudio.Play();
    }

    public void PlayFriendsItemAudio()
    {
        friendsItemAudio.Stop();
        friendsItemAudio.Play();
    }
}
