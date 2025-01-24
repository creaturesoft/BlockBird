using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static float InitCameraX;

    private float moveSpeed = 0.3f; // 이동 속도


    void Start()
    {
        InitCameraX = Screen.width / Screen.height * 3.5f;
        //transform.position = new Vector3(InitCameraX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        float targetX = GameManager.Instance.Character.transform.position.x;
        float targetY = GameManager.Instance.Character.transform.position.y;

        //게임오버
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
            //제자리일 때는 카메라 무빙하지 않음

            //카메라 위치가 다를 때만 이동
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

        //밀리고 있을 때 카메라 무빙
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
