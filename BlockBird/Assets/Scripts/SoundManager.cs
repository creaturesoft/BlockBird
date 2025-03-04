using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource[] breakBlockAudio;
    public AudioSource[] breakBossBlockAudio;

    public AudioSource startAudio;
    public AudioSource getItemAudio;
    public AudioSource[] successAudio;

    public AudioSource speedUpItemAudio;
    public AudioSource friendsItemAudio;

    public AudioMixer audioMixer; // Audio Mixer를 인스펙터에서 연결

    public bool isBGMOn { get; set; } = true;
    public bool isSFXOn { get; set; } = true;
    public float BGMVolume { get; private set; }
    public float SFXVolume { get; private set; }

    public void SetBGMVolume(float volume)
    {
        BGMVolume = volume;
        if (volume < 0.0002f)
        {
            isBGMOn = false;
        }
        else
        {
            isBGMOn = true;
        }

        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20 + 10); // 데시벨 변환
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        if (volume < 0.0002f)
        {
            isSFXOn = false;
        }
        else
        {
            isSFXOn = true;
        }

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20 + 10);
    }

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
        if (Time.time - GameManager.blockSoundLastPlayTime >= GameManager.blockSoundPlayCooldown && SoundManager.Instance.isSFXOn)
        {

            int index = Random.Range(0, breakBlockAudio.Length);
            if (isBoss)
            {

                breakBossBlockAudio[index].Stop();
                breakBossBlockAudio[index].Play();

            }
            else
            {
                breakBlockAudio[index].Stop();
                breakBlockAudio[index].Play();
            }

            GameManager.blockSoundLastPlayTime = Time.time;
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
        int index = Random.Range(0, successAudio.Length);
        successAudio[index].Stop();
        successAudio[index].Play();
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
