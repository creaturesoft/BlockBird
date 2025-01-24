using UnityEngine;

public class DepthBase : MonoBehaviour
{
    public float BlockGap { get; set; } = 1.2f;
    
    public int Steps { get; set; } = 3;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.Character != null)
        {
            transform.position += Vector3.left * Time.fixedDeltaTime * GameManager.Instance.Character.Speed;

            if(transform.position.x < -50)
            {
                Destroy(gameObject);
            }
        }
    }
}
