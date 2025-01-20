using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거에 들어온 객체를 파괴
        Destroy(other.gameObject);
    }
}
