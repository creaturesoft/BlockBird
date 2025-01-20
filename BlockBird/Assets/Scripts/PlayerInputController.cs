using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private float jumpForce;

    [Header("Components")]
    public AudioSource jumpSound; // 점프 효과음 (선택 사항)

    private Rigidbody2D rb; // 캐릭터의 Rigidbody2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpForce = GetComponent<Character>().JumpForce;
    }

    void Update()
    {

    }

    void OnAttack()
    {
        if(GameManager.Instance.IsGameOver)
        {
            return;
        }

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (jumpSound != null)
        {
            jumpSound.Play();
        }
    }
}
