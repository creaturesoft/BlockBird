using UnityEngine;

public class DepthBase : MonoBehaviour
{
    public float BlockGap { get; set; } = 1.2f;
    
    public int Steps { get; set; } = 3;

    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {            
        if (collision.CompareTag("Player") && !GameManager.Instance.Character.IsDie)
        {
            GameManager.Instance.Character.Die(true);
        }
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.Character != null)
        {
            transform.position += Vector3.left * Time.fixedDeltaTime * GameManager.Instance.Character.Speed;

            //if(transform.position.x < -30)
            //{
            //    Destroy(gameObject);
            //}
        }
    }
}
