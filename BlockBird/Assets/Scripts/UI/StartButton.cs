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

        //���� ĳ����
        if (CharacterSelect.SelectedCharacter == 0)
        {
            characterSelect.SetRandomCharacter();
        }

        Vector3 initialScale = GameManager.Instance.Character.transform.localScale;
        Vector3 targetScale = initialScale / 2f; // 2�迡�� 1��� �پ��
        float duration = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // ũ�⸦ Lerp�� ���������� ���̱�
            GameManager.Instance.Character.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);

            //ī�޶� ��ġ �̵�
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position
            , new Vector3(
                CameraFollow.InitCameraX
                , Camera.main.transform.position.y
                , Camera.main.transform.position.z)
            , 2f * Time.unscaledDeltaTime);

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime); // �ǽð� ���
        }

        Time.timeScale = 1;

        // ���������� ��ǥ ũ��� ����
        GameManager.Instance.Character.transform.localScale = targetScale;
        GameManager.Instance.Character.EnableCharacter();
        map.SetActive(true);
    }

}
