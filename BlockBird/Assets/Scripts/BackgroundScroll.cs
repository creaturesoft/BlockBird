using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speedReduce = 2f;         // 배경 이동 속도
    private float backgroundWidth;  // 배경의 너비 (자동 계산)
    private Vector3 startPosition;  // 초기 위치 저장

    void Start()
    {
        // SpriteRenderer를 통해 배경 너비 계산
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            backgroundWidth = spriteRenderer.bounds.size.x; // X축 크기 계산
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on this GameObject!");
        }

        // 초기 위치 저장
        startPosition = transform.position;
    }

    void Update()
    {
        if(GameManager.Instance.Character == null)
        {
            return;
        }

        // 배경 이동
        transform.Translate(GameManager.Instance.Character.Speed / speedReduce * Vector3.left * Time.deltaTime);

        // 배경이 화면 밖으로 나가면 위치 재배치
        if (transform.position.x <= startPosition.x - backgroundWidth)
        {
            transform.position += new Vector3(backgroundWidth, 0, 0);
        }
    }
}
