using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject); // �ڽ�(GameObject)�� ����
    }

    public void DestroyAndPlay()
    {
        Time.timeScale = 1;
        Destroy(gameObject); // �ڽ�(GameObject)�� ����
    }
}
