using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameObject introButton;
    public GameObject map;
    public CharacterSelect characterSelect;


    public void OnStart()
    {
        introButton.SetActive(false);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        Time.timeScale = 0;

        //랜덤 캐릭터
        if (CharacterSelect.SelectedCharacter == 0)
        {
            characterSelect.SetRandomCharacter();
        }

        Vector3 initialScale = GameManager.Instance.Character.transform.localScale;
        Vector3 targetScale = initialScale / 2f; // 2배에서 1배로 줄어듦
        float duration = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // 크기를 Lerp로 점진적으로 줄이기
            GameManager.Instance.Character.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);

            //카메라 위치 이동
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position
            , new Vector3(
                CameraFollow.InitCameraX
                , Camera.main.transform.position.y
                , Camera.main.transform.position.z)
            , 2f * Time.unscaledDeltaTime);

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime); // 실시간 대기
        }

        Time.timeScale = 1;

        // 최종적으로 목표 크기로 설정
        GameManager.Instance.Character.transform.localScale = targetScale;
        GameManager.Instance.Character.EnableCharacter();
        map.SetActive(true);
    }

}
