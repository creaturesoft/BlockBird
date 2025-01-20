using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour
{
    public GameObject introButton;
    public GameObject map;

    public void OnStart()
    {
        introButton.SetActive(false);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        Vector3 initialScale = GameManager.Instance.Character.transform.localScale;
        Vector3 targetScale = initialScale / 2f; // 2배에서 1배로 줄어듦
        float duration = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // 크기를 Lerp로 점진적으로 줄이기
            GameManager.Instance.Character.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);

            //카메라 위치 이동
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position
            , new Vector3(
                CameraFollow.InitCameraX
                , Camera.main.transform.position.y
                , Camera.main.transform.position.z)
            , 2f * Time.deltaTime);

            yield return null; // 다음 프레임까지 대기
        }

        // 최종적으로 목표 크기로 설정
        GameManager.Instance.Character.transform.localScale = targetScale;
        GameManager.IsPaused = false;
        GameManager.Instance.Character.EnableCharacter();
        map.SetActive(true);
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
