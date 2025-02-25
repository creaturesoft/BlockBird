using I2.Loc;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static GameManager;

public class CharacterDraw : MonoBehaviour
{
    public GameObject characterImages;
    public GameObject winEffect;
    public GameObject drawButton;
    public List<Button> drawButtonList;

    public GameObject newCharacterText;
    public GameObject levelUpText;

    public TextMeshProUGUI characterText;
    public TextMeshProUGUI gemText;

    public bool isIntro;

    private static readonly int gemPrice = 100;

    public AudioSource audio1;
    public AudioSource audio2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isIntro = GameObject.Find("IntroButton") == null ? false : true;

        SetCharacterText();
        SetGemText();

        GameManager.Instance.UserDataChanged += SetGemText;
        GameManager.Instance.LoadGuestGem();
    }


    private void OnDestroy()
    {
        GameManager.Instance.UserDataChanged -= SetGemText;
    }


    void SetCharacterText()
    {
        characterText.text = GameManager.Instance.UserData.birdList.Count() + " / ??";
        //characterText.text = GameManager.Instance.UserData.birdList.Count() + " / " + (GameManager.Instance.characterSelect.AllCharactersPrefabs.Length - 2);
    }

    void SetGemText()
    {
        gemText.text = "X " + GameManager.Instance.UserData.gem.ToString();
    }

    public void OnCharacterDraw(int buttonIndex)
    {
        if (!PageController.Instance.CheckInternetConnection())
        {
            return;
        }


        newCharacterText.SetActive(false);
        levelUpText.SetActive(false);

        if (GameManager.Instance.UserData.gem < gemPrice)
        {
            PageController.Instance.ShowShop();
            return;
        }


        audio1.Play();

        //뽑기는 서버에서. 최대 캐릭터 개수만 서버로 보냄
        //characterImages.transform.childCount
        //뽑은 캐릭터 서버에 등록
        //뽑은 캐릭터 받아옴

        if (buttonIndex >= 0)
        {
            drawButtonList[buttonIndex].gameObject.SetActive(false);
        }

        foreach (Button button in drawButtonList)
        {
            button.enabled = false;
        }

        winEffect.gameObject.SetActive(false);

        foreach (Transform child in characterImages.transform)
        {
            child.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            child.gameObject.SetActive(false);
        }

        StartCoroutine(RollingCharacterImage());
    }

    IEnumerator RollingCharacterImage()
    {
        int currentImage = 0;
        int previousImage = characterImages.transform.childCount - 1;

        int currentCount = 0;
        int maxCount = Random.Range(20,30);

        while (currentCount < maxCount)
        {
            currentCount++;

            if (currentImage >= characterImages.transform.childCount)
            {
                currentImage = 0;
            }

            if (previousImage >= characterImages.transform.childCount)
            {
                previousImage = 0;
            }

            characterImages.transform.GetChild(currentImage).gameObject.SetActive(true);
            characterImages.transform.GetChild(previousImage).gameObject.SetActive(false);

            currentImage++;
            previousImage++;

            yield return new WaitForSecondsRealtime(0.03f + (float)currentCount/200);
            if(maxCount - currentCount < 4)
            {
                yield return new WaitForSecondsRealtime(0.15f);
            }
            if (maxCount - currentCount < 2)
            {
                yield return new WaitForSecondsRealtime(0.4f);
            }
        }

        characterImages.transform.GetChild(currentImage-1).gameObject.SetActive(false);

        int win = WinCharacter();

        GameObject winCharacter = characterImages.transform.GetChild(win).gameObject;
        winCharacter.SetActive(true);
        winCharacter.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        winEffect.gameObject.SetActive(true);

        //서버에서 userData 가져옴
        User serverUser = null;
        yield return SaveLoadManager.LoadUserDataFromServer(GameManager.Instance.UserData.userId, (result) => {
            serverUser = result;
        });
            
        if (serverUser == null)
        {
            Debug.Log("!!!!!!!!!!!!!!! 서버에서 데이터 가져오기 실패 !!!!!!!!!!!!!!!");
            yield break;
        }
        else
        {
            if (GameManager.Instance.UserData.isGuest)
            {
                GameManager.Instance.UserData.gem = serverUser.gem;
            }
            else
            {
                GameManager.Instance.UserData = serverUser;
            }
        }

        if (GameManager.Instance.UserData.gem < gemPrice)
        {
            PageController.Instance.ShowShop();

            foreach (Button button in drawButtonList)
            {
                button.enabled = true;
            }
            yield break;
        }


        audio2.Play();

        BirdData winBirdData = GameManager.Instance.UserData.birdList.Where(x => x.name == winCharacter.name).FirstOrDefault();
        if(winBirdData == null)
        {
            //새로운 캐릭터
            winBirdData = new BirdData();
            winBirdData.name = winCharacter.name;
            winBirdData.expLevel = 1;
            GameManager.Instance.UserData.birdList.Add(winBirdData);

            newCharacterText.SetActive(true);
        }
        else 
        {
            //레벨업
            winBirdData.pullLevel += 1;

            levelUpText.SetActive(true);
        }

        SpendGem(gemPrice);
        SaveLoadManager.SaveUserData(GameManager.Instance.UserData);    //캐릭터 저장
        StartCoroutine(SaveLoadManager.SendUserDataToServer(GameManager.Instance.UserData));

        if (isIntro)
        {
            GameManager.Instance.characterSelect.Init();
        }

        SetCharacterText();
        //SetGemText();

        foreach (Button button in drawButtonList)
        {
            button.enabled = true;
        }

    }

    int WinCharacter()
    {
        int win = 1;


        int rate = 2;

        //if((float)GameManager.Instance.UserData.birdList.Count() / (float)(characterImages.transform.childCount-1) < 0.8f)
        //{
        //    remainCharacter = 2;
        //}
        //else
        //{
        //    remainCharacter = 1;
        //}


        //Debug.Log(remainCharacter);

        for (int i = 0; i < rate; i++)
        {
            win = Random.Range(1, characterImages.transform.childCount);
            GameObject winCharacter = characterImages.transform.GetChild(win).gameObject;
            BirdData winBirdData = GameManager.Instance.UserData.birdList.Where(x => x.name == winCharacter.name).FirstOrDefault();
            if (winBirdData == null)
            {
                //새로운 캐릭터
                continue;
            }
            else
            {
                //레벨업
                break;
            }
        }


        //float test = 0;
        //for (int j = 0; j < 10000; j++)
        //{
        //    for (int i = 0; i < remainCharacter; i++)
        //    {
        //        win = Random.Range(1, characterImages.transform.childCount);
        //        GameObject winCharacter = characterImages.transform.GetChild(win).gameObject;
        //        BirdData winBirdData = GameManager.Instance.UserData.birdList.Where(x => x.name == winCharacter.name).FirstOrDefault();
        //        if (winBirdData == null)
        //        {
        //            //새로운 캐릭터
        //            continue;
        //        }
        //        else
        //        {
        //            //레벨업
        //            break;
        //        }
        //    }

        //    GameObject winCharacter2 = characterImages.transform.GetChild(win).gameObject;
        //    BirdData winBirdData2 = GameManager.Instance.UserData.birdList.Where(x => x.name == winCharacter2.name).FirstOrDefault();
        //    if (winBirdData2 == null)
        //    {
        //        test += 1;
        //    }
        //}

        //Debug.Log(test / 10000f * 100f);

        return win;
    }

    void SpendGem(int amount)
    {
        string data = "{\"userId\": \"" + GameManager.Instance.UserData.userId + "\", " +
        "\"type\": \"SPEND\", " +
        "\"amount\": " + amount + "}";

        StartCoroutine(WebRequest.Post(WebRequest.GemTransactionURL, data, (result) =>
        {
            if (result == null)
            {
                //실패
                PersistentObject.Instance.ShowMessagePopup(0, () => { });
            }
            else
            {
                //성공
                GameManager.Instance.UserData.gem -= (int)result.GetValue("amount");
                GameManager.Instance.UpdateUserData();
            }
        }));
    }
}
