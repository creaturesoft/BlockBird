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
        Vector3 targetScale = initialScale / 2f; // 2�迡�� 1��� �پ��
        float duration = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // ũ�⸦ Lerp�� ���������� ���̱�
            GameManager.Instance.Character.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);

            //ī�޶� ��ġ �̵�
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position
            , new Vector3(
                CameraFollow.InitCameraX
                , Camera.main.transform.position.y
                , Camera.main.transform.position.z)
            , 2f * Time.deltaTime);

            yield return null; // ���� �����ӱ��� ���
        }

        // ���������� ��ǥ ũ��� ����
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
