using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;


public class Setting : MonoBehaviour
{
    public Slider StageSlider;
    public TextMeshProUGUI stageText;


    public Slider SFXSlider;
    public Slider BGMSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject reviewButton;
    bool isFirst = true;

    void Start()
    {

        SFXSlider.value = SoundManager.Instance.SFXVolume;
        BGMSlider.value = SoundManager.Instance.BGMVolume;

        if (PersistentObject.Instance.UserData.isReviewed)
        {
            reviewButton.SetActive(false);
        }
        else
        {
            reviewButton.SetActive(true);
        }
    }


    public void OnStageChanged()
    {
        if (isFirst)
        {
            isFirst = false;
            StageSlider.maxValue = PersistentObject.Instance.UserData.maxStage;
            StageSlider.value = PersistentObject.Instance.UserData.stage;
        }

        stageText.text = "Stage " + StageSlider.value;
    }

    public void OnBGMVolumeChanged(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
    }

    public void SaveSetting()
    {
        PersistentObject.Instance.setting.SFXVolume = SoundManager.Instance.SFXVolume;
        PersistentObject.Instance.setting.BGMVolume = SoundManager.Instance.BGMVolume;
        PlayerPrefsManager.SaveSetting(PersistentObject.Instance.setting);


        if (PersistentObject.Instance.UserData.stage != (int)StageSlider.value)
        {
            PersistentObject.Instance.UserData.stage = (int)StageSlider.value;
            SaveLoadManager.SaveUserData(PersistentObject.Instance.UserData);
            StartCoroutine(SaveLoadManager.SendUserDataToServer(PersistentObject.Instance.UserData));

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnShare()
    {
        SunShineNativeShare.instance.ShareText(PersistentObject.Instance.GetStoreURL(), "Block Birds");
    }

}
