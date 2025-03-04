using System.Collections;
using UnityEngine;

public class FlamingoGunBullet : Bullet
{
    public Transform centerObject;  // ȸ�� �߽��� �Ǵ� ������Ʈ
    //public float radius = 1.3f;       // ���� �˵��� ������
    public float radius = 0f;       // ���� �˵��� ������

    public float currentAngle = 0f; // ���� ���� (���� ������ ����)


    protected override void Start()
    {
        base.Start();


        //radius = GameManager.Instance.Character.gameObject.GetComponent<CircleCollider2D>().radius * 3f;
    }

    protected override void FixedUpdate()
    {
        if(centerObject == null)
        {
            return;
        }

        Life = 1000;
        

        // ���� ������Ʈ (�ʴ� angularSpeed��ŭ ���� ����)
        currentAngle += Speed * Time.fixedDeltaTime;

        // �� ����(360�� = 2�� ����)�� ���� �ʱ�ȭ
        //if (currentAngle >= Mathf.PI * 2f)
        //{
        //    currentAngle -= Mathf.PI * 2f;  // �� ���� �������Ƿ� �ʱ�ȭ
        //}

        // x, y ��ǥ ��� (���� ������ - 2D ����)
        float x = centerObject.position.x + Mathf.Cos(currentAngle) * radius;
        float y = centerObject.position.y + Mathf.Sin(currentAngle) * radius;

        // ���ο� ��ġ�� �̵�
        rb.MovePosition(new Vector3(x, y, transform.position.z));
    }

    protected override float SpecialEffect(BlockBase block)
    {
        if (block.IsBoss)
        {
            return Damage * 2;
        }
        else
        {
            return Damage;
        }
    }
}
