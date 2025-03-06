using UnityEngine;
using System.Collections;

public class PlayerInputController : MonoBehaviour
{
    private float jumpForce;

    [Header("Components")]
    public AudioSource jumpSound; // 점프 효과음 (선택 사항)

    private Rigidbody2D rb; // 캐릭터의 Rigidbody2D

    private bool isBackPressedOnce = false;  // 뒤로가기 버튼 한 번 눌림 여부 체크
    private float doublePressDelay = 1.5f;   // 두 번째 버튼 입력 허용 시간

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

        foreach (Rigidbody2D friendRb in GameManager.Instance.friendRbList)
        {
            friendRb.linearVelocity = Vector2.zero;
            friendRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnExit()
    {
        Debug.Log("Exit");

        if (isBackPressedOnce)
        {
            // 두 번째로 눌렸을 때 게임 종료
            Debug.Log("게임 종료");

//#if UNITY_ANDROID
//            Application.Quit();
//            System.Diagnostics.Process.GetCurrentProcess().Kill(); // 강제 종료 (Android에서 사용 가능)
//#endif
        }
        else
        {
            isBackPressedOnce = true;
            Debug.Log("뒤로가기 버튼을 한 번 더 누르면 종료됩니다.");

            // 일정 시간 내에 두 번째 입력이 없으면 상태 초기화
            StartCoroutine(ResetBackPress());
        }
    }


    private IEnumerator ResetBackPress()
    {
        yield return new WaitForSeconds(doublePressDelay);
        isBackPressedOnce = false;
    }
}
