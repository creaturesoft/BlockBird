using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDraw : MonoBehaviour
{
    public GameObject characterImages;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCharacterDraw()
    {
        StartCoroutine(RollingCharacterImage());
    }

    IEnumerator RollingCharacterImage()
    {
        int currentImage = 0;
        int previousImage = characterImages.transform.childCount - 1;

        int currentCount = 0;
        int maxCount = 30;
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

            yield return new WaitForSeconds(0.1f + currentCount/30);
        }

        characterImages.transform.GetChild(currentImage - 1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
}
