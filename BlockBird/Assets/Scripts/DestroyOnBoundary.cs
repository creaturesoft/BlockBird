using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ʈ���ſ� ���� ��ü�� �ı�
        Destroy(other.gameObject);
    }
}
