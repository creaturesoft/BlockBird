using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speedReduce = 2f;         // ��� �̵� �ӵ�
    private float backgroundWidth;  // ����� �ʺ� (�ڵ� ���)
    private Vector3 startPosition;  // �ʱ� ��ġ ����

    void Start()
    {
        // SpriteRenderer�� ���� ��� �ʺ� ���
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            backgroundWidth = spriteRenderer.bounds.size.x; // X�� ũ�� ���
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on this GameObject!");
        }

        // �ʱ� ��ġ ����
        startPosition = transform.position;
    }

    void Update()
    {
        if(GameManager.Instance.Character == null)
        {
            return;
        }

        // ��� �̵�
        transform.Translate(GameManager.Instance.Character.Speed / speedReduce * Vector3.left * Time.deltaTime);

        // ����� ȭ�� ������ ������ ��ġ ���ġ
        if (transform.position.x <= startPosition.x - backgroundWidth)
        {
            transform.position += new Vector3(backgroundWidth, 0, 0);
        }
    }
}
