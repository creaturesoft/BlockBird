using System.Collections;
using UnityEngine;

public class FlamingoGunBullet : Bullet
{
    private Transform centerObject;  // ȸ�� �߽��� �Ǵ� ������Ʈ
    private float radius = 1f;       // ���� �˵��� ������

    private float currentAngle = 0f; // ���� ���� (���� ������ ����)

    protected override void Start()
    {
        base.Start();


        radius = GameManager.Instance.Character.gameObject.GetComponent<CircleCollider2D>().radius * 3f;
    }

    protected override void FixedUpdate()
    {
        Life = 100;

        centerObject = GameManager.Instance.Character.gameObject.transform;

        // ���� ������Ʈ (�ʴ� angularSpeed��ŭ ���� ����)
        currentAngle += Speed * Time.fixedDeltaTime;

        // �� ����(360�� = 2�� ����)�� ���� �ʱ�ȭ
        if (currentAngle >= Mathf.PI * 2f)
        {
            currentAngle -= Mathf.PI * 2f;  // �� ���� �������Ƿ� �ʱ�ȭ
        }

        // x, y ��ǥ ��� (���� ������ - 2D ����)
        float x = centerObject.position.x + Mathf.Cos(currentAngle) * radius;
        float y = centerObject.position.y + Mathf.Sin(currentAngle) * radius;

        // ���ο� ��ġ�� �̵�
        rb.MovePosition(new Vector3(x, y, transform.position.z));
    }
}
