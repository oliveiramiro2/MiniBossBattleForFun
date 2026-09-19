using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
  private float _damage = 15f;
  private Rigidbody2D _rb;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
  }

  public void Initialize(Vector2 direction, float speed, float damage)
  {
    _damage = damage;
    _rb.linearVelocity = direction.normalized * speed;
    Destroy(gameObject, 5f); // Destrói após 5 segundos se não acertar nada
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      PlayerController player = collision.GetComponent<PlayerController>();
      HealthSystem playerHealth = collision.GetComponent<HealthSystem>();

      if (playerHealth != null)
      {
        playerHealth.TakeDamage(_damage, true, player);
      }
      Destroy(gameObject);
    }
    else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      Destroy(gameObject);
    }
  }
}