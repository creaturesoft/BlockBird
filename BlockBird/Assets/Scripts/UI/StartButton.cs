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
            60f,      //�ƹ��͵� ����
            30f,      //����
            5f,      //�������� ��õ
            5f,      //����
    };

    public void OnStart()
    {
        //���ο� ĳ����
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
                if (i == 0)         //�ƹ��͵� ����
                {
                    break;
                }
                if (i == 1)         //����
                {
                    if (!PersistentObject.Instance.IsNoAd)
                    {
                        PersistentObject.Instance.interstitialAdManager.ShowInterstitial();
                        while (PersistentObject.Instance.interstitialAdManager.IsPlaying)
                        {
                            yield return new WaitForSecondsRealtime(0.2f); // �ǽð� ���
                        }
                    }
                }
                else if (i == 2)    //��������
                {
                    //��������
                    if (!PersistentObject.Instance.IsNoAd && Application.internetReachability != NetworkReachability.NotReachable)
                    {
                        AdRemove adRemove = PageController.Instance.ShowAdRemove();
                        while (adRemove.IsPlaying)
                        {
                            yield return new WaitForSecondsRealtime(0.2f); // �ǽð� ���
                        }
                    }
                }
                else if (i == 3)  
                {
                    //����
                    if (!PersistentObject.Instance.UserData.isReviewed && Application.internetReachability != NetworkReachability.NotReachable)
                    {
                        ReviewManagerScript review = PageController.Instance.ShowReview();
                        while (review.IsPlaying)
                        {
                            yield return new WaitForSecondsRealtime(0.2f); // �ǽð� ���
                        }
                    }
                }

                break;
            }
        }


        //���� ĳ����
        if (CharacterSelect.SelectedCharacter == 1)
        {
            characterSelect.SetRandomCharacter();
        }

        Vector3 initialScale = GameManager.Instance.Character.transform.localScale;
        Vector3 initialPosition = GameManager.Instance.Character.transform.position;
        
        Vector3 targetScale = initialScale / 2f; // 2�迡�� 1��� �پ��
        Vector3 targetPosition = Vector3.zero;

        float duration = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;

            // ũ�⸦ Lerp�� ���������� ���̱�
            GameManager.Instance.Character.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            GameManager.Instance.Character.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);

            //ī�޶� ��ġ �̵�
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position
            , new Vector3(
                CameraFollow.InitCameraX
                , Camera.main.transform.position.y
                , Camera.main.transform.position.z)
            , 2f * Time.fixedDeltaTime);

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime); // �ǽð� ���
        }

        Time.timeScale = 1;

        // ���������� ��ǥ ũ��� ����
        GameManager.Instance.Character.transform.localScale = targetScale;
        GameManager.Instance.Character.EnableCharacter();

        MapManager.Instance.StartMap();
    }

}
