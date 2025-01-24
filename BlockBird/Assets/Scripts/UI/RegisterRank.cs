using UnityEngine;
using System.Collections;

public class RegisterRank : MonoBehaviour
{
    public void Register()
    {
        StartCoroutine(ProcessRegister());
    }

    IEnumerator ProcessRegister()
    {
        PersistentObject.Instance.interstitialAdManager.ShowInterstitial();

        while (PersistentObject.Instance.interstitialAdManager.IsPlaying)
        {
            yield return new WaitForSecondsRealtime(0.2f); // 실시간 대기
        }

        // 기본 웹브라우저로 URL 열기
        Application.OpenURL("https://www.google.com");

        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
