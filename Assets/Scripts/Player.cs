using UnityEngine;

public class Player : MonoBehaviour
{
    public float movespeed = 3f;
    public GameObject shoottrans;
    private Rigidbody2D rb;
    private Vector2 MoveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (MoveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = shoottrans.transform.position;
            // 플레이어가 바라보는 shoottrans의 up 방향으로 총알 발사
            bullet.GetComponent<Bullet>().Fire(shoottrans.transform.up);
            GameManager.Instance.IncreaseScore(10);
        }

        float movex = Input.GetAxisRaw("Horizontal");
        float movey = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector2(movex, movey).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = MoveDirection * movespeed;
    }
}
