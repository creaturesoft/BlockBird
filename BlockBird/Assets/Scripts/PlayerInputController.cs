using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private float jumpForce = 7f; // 점프 힘

    [Header("Components")]
    public AudioSource jumpSound; // 점프 효과음 (선택 사항)

    private Rigidbody2D rb; // 캐릭터의 Rigidbody2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    void OnAttack()
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if(jumpSound != null)
        {
            jumpSound.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("Game Over!");
            // 게임 오버 처리
        }
    }
}
