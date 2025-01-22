using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject); // 자신(GameObject)을 삭제
    }

    public void DestroyAndPlay()
    {
        Time.timeScale = 1;
        Destroy(gameObject); // 자신(GameObject)을 삭제
    }
}
