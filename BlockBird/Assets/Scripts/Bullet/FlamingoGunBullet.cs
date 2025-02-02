using System.Collections;
using UnityEngine;

public class FlamingoGunBullet : Bullet
{
    private Transform centerObject;  // 회전 중심이 되는 오브젝트
    private float radius = 1f;       // 원형 궤도의 반지름

    private float currentAngle = 0f; // 현재 각도 (라디안 값으로 관리)

    protected override void Start()
    {
        base.Start();


        radius = GameManager.Instance.Character.gameObject.GetComponent<CircleCollider2D>().radius * 3f;
    }

    protected override void FixedUpdate()
    {
        Life = 100;

        centerObject = GameManager.Instance.Character.gameObject.transform;

        // 각도 업데이트 (초당 angularSpeed만큼 각도 증가)
        currentAngle += Speed * Time.fixedDeltaTime;

        // 한 바퀴(360도 = 2π 라디안)를 돌면 초기화
        if (currentAngle >= Mathf.PI * 2f)
        {
            currentAngle -= Mathf.PI * 2f;  // 한 바퀴 돌았으므로 초기화
        }

        // x, y 좌표 계산 (원의 방정식 - 2D 버전)
        float x = centerObject.position.x + Mathf.Cos(currentAngle) * radius;
        float y = centerObject.position.y + Mathf.Sin(currentAngle) * radius;

        // 새로운 위치로 이동
        rb.MovePosition(new Vector3(x, y, transform.position.z));
    }
}
