using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static float InitCameraX;

    private float moveSpeed = 0.3f; // �̵� �ӵ�


    void Start()
    {
        InitCameraX = Screen.width / Screen.height * 3.5f;
        //transform.position = new Vector3(InitCameraX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        float targetX = GameManager.Instance.Character.transform.position.x;
        float targetY = GameManager.Instance.Character.transform.position.y;

        //���ӿ���
        if (GameManager.Instance.IsResultProcess)
        {
            transform.position = Vector3.Lerp(transform.position
                , new Vector3(
                      targetX
                    , targetY - 3f
                    , transform.position.z)
                , 1.5f * Time.deltaTime);

            return;
        }

        if (!GameManager.Instance.Character.isActive)
        {
            return;
        }

        if (targetX > -0.01f && targetX < 0.01f)
        {
            //���ڸ��� ���� ī�޶� �������� ����

            //ī�޶� ��ġ�� �ٸ� ���� �̵�
            if (transform.position.x - InitCameraX > 0.01f ||
                transform.position.x - InitCameraX < -0.01f
                )
            {
                transform.position = Vector3.Lerp(transform.position
                , new Vector3(
                    InitCameraX
                    , transform.position.y
                    , transform.position.z)
                , 2f * Time.deltaTime);
            }

            return;
        }

        //�и��� ���� �� ī�޶� ����
        if (targetX < -1.5f || targetX > 1.5f)
        {
            transform.position = new Vector3(
                InitCameraX + targetX + 1.5f
                , transform.position.y
                , transform.position.z);

            //    transform.position = Vector3.Lerp(transform.position
            //    , new Vector3(
            //        InitCameraX + targetX
            //        , transform.position.y
            //        , transform.position.z)
            //    , 2f * Time.deltaTime);
        }
    }
}
