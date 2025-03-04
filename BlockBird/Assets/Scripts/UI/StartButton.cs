using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameObject introButton;
    public GameObject signInButton;
    public GameObject homeButton;
    public GameObject settingButton;
    public CharacterSelect characterSelect;

    float[] weightList = {
            60f,      //아무것도 안함
            30f,      //광고
            5f,      //광고제거 추천
            5f,      //리뷰
    };

    public void OnStart()
    {
        //새로운 캐릭터
        if (CharacterSelect.SelectedCharacter == 0)
        {
            PageController.Instance.ShowCharacterDraw();
            return;
        }

        introButton.SetActive(false);
        signInButton.SetActive(false);
        settingButton.SetActive(false);
        homeButton.SetActive(true);
        StartCoroutine(StartGame());
    }


    IEnumerator StartGame()
    {
        Time.timeScale = 0;

        float totalWeight = 0;
        foreach (float randomRate in weightList)
        {
            totalWeight += randomRate;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float currentWeight = 0;

        for (int i = 0; i < weightList.Length; i++)
        {
            currentWeight += weightList[i];
            if (randomValue < currentWeight)
            {
                if (i == 0)         //아무것도 안함
                {
                    break;
                }
                if (i == 1)         //광고
                {
                    if (!PersistentObject.Instance.IsNoAd)
                    {
                        PersistentObject.Instance.interstitialAdManager.ShowInterstitial();
                        while (PersistentObject.Instance.interstitialAdManager.IsPlaying)
                        {
                            yield return new WaitForSecondsRealtime(0.2f); // 실시간 대기
                        }
                    }
                }
                else if (i == 2)    //광고제거
                {
                    //광고제거
                    if (!PersistentObject.Instance.IsNoAd && Application.internetReachability != NetworkReachability.NotReachable)
                    {
                        AdRemove adRemove = PageController.Instance.ShowAdRemove();
                        while (adRemove.IsPlaying)
                        {
                            yield return new WaitForSecondsRealtime(0.2f); // 실시간 대기
                        }
                    }
                }
                else if (i == 3)  
                {
                    //리뷰
                    if (!PersistentObject.Instance.UserData.isReviewed && Application.internetReachability != NetworkReachability.NotReachable)
                    {
                        ReviewManagerScript review = PageController.Instance.ShowReview();
                        while (review.IsPlaying)
                        {
                            yield return new WaitForSecondsRealtime(0.2f); // 실시간 대기
                        }
                    }
                }

                break;
            }
        }


        //랜덤 캐릭터
        if (CharacterSelect.SelectedCharacter == 1)
        {
            characterSelect.SetRandomCharacter();
        }

        Vector3 initialScale = GameManager.Instance.Character.transform.localScale;
        Vector3 initialPosition = GameManager.Instance.Character.transform.position;
        
        Vector3 targetScale = initialScale / 2f; // 2배에서 1배로 줄어듦
        Vector3 targetPosition = Vector3.zero;

        float duration = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;

            // 크기를 Lerp로 점진적으로 줄이기
            GameManager.Instance.Character.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            GameManager.Instance.Character.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);

            //카메라 위치 이동
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position
            , new Vector3(
                CameraFollow.InitCameraX
                , Camera.main.transform.position.y
                , Camera.main.transform.position.z)
            , 2f * Time.fixedDeltaTime);

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime); // 실시간 대기
        }

        Time.timeScale = 1;

        // 최종적으로 목표 크기로 설정
        GameManager.Instance.Character.transform.localScale = targetScale;
        GameManager.Instance.Character.EnableCharacter();

        MapManager.Instance.StartMap();
    }

}
