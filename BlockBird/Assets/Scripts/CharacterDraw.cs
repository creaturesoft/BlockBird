using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class CharacterDraw : MonoBehaviour
{
    public GameObject characterImages;
    public GameObject winEffect;
    public GameObject drawButton;
    private List<Button> drawButtonList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        drawButtonList = new List<Button>();
        foreach (Transform child in drawButton.transform)
        {
            child.gameObject.SetActive(true);
            drawButtonList.Add(child.GetComponent<Button>());
        }
    }

    public void OnCharacterDraw()
    {
        if (!PageController.Instance.CheckInternetConnection())
        {
            return;
        }

        //뽑기는 서버에서. 최대 캐릭터 개수만 서버로 보냄
        //characterImages.transform.childCount
        //뽑은 캐릭터 서버에 등록
        //뽑은 캐릭터 받아옴

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
        int maxCount = Random.Range(30,40);
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

            yield return new WaitForSecondsRealtime(0.05f + (float)currentCount/200);
            if(maxCount - currentCount < 4)
            {
                yield return new WaitForSecondsRealtime(0.2f);
            }
            if (maxCount - currentCount < 2)
            {
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }

        characterImages.transform.GetChild(currentImage-1).gameObject.SetActive(false);

        int winCharacter = Random.Range(0, characterImages.transform.childCount);   //서버에서 받아온 캐릭터로 바꿈
        characterImages.transform.GetChild(winCharacter).gameObject.SetActive(true);
        characterImages.transform.GetChild(winCharacter).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        winEffect.gameObject.SetActive(true);

        foreach (Button button in drawButtonList)
        {
            button.enabled = true;
        }
    }
}
