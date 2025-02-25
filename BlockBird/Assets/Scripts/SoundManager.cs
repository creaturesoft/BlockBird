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
        // 싱글톤 인스턴스가 이미 존재하면 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스를 현재 객체로 설정
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
