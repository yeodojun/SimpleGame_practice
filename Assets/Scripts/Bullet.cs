using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float trailTime = 0.5f;
    public float collisionOffset = 0.1f;
    public float collisionCooldown = 0.1f;
    
    private Rigidbody2D rb;
    private bool recentlyCollided = false;
    private bool hasBounced = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        TrailRenderer tr = GetComponent<TrailRenderer>();
        if (tr == null)
        {
            tr = gameObject.AddComponent<TrailRenderer>();
        }
        tr.time = trailTime;
        tr.startWidth = 0.1f;
        tr.endWidth = 0.0f;
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.startColor = Color.white;
        tr.endColor = new Color(1, 1, 1, 0);

        gameObject.layer = LayerMask.NameToLayer("Bullet");
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer, true);
    }
    
    public void Fire(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        rb.linearVelocity = direction.normalized * speed;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!hasBounced)
                return;
            Destroy(collision.gameObject);
            GameManager.Instance.GameOver();
            return;
        }
        if (recentlyCollided)
            return;
        recentlyCollided = true;
        StartCoroutine(ResetCollisionCooldown());

        // 충돌면의 법선을 기준으로 반사 벡터 계산
        Vector2 reflectDir = Vector2.Reflect(rb.linearVelocity, collision.contacts[0].normal).normalized;
        rb.linearVelocity = reflectDir * speed;

        // 충돌면의 법선 방향으로 약간 오프셋 적용하여 벽에 붙는 현상 방지
        transform.position += (Vector3)collision.contacts[0].normal * collisionOffset;

        // 이동 방향에 맞춰 회전 업데이트
        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        // 벽(혹은 다른 오브젝트)와 충돌하여 반사한 적이 있음을 기록
        hasBounced = true;
    }
    
    IEnumerator ResetCollisionCooldown()
    {
        yield return new WaitForSeconds(collisionCooldown);
        recentlyCollided = false;
    }
}
