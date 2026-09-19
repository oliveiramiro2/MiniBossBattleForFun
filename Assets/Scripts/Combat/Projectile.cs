using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
  [SerializeField] private float speed = 12f;
  [SerializeField] private float damage = 15f;
  private Rigidbody2D _rb;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
  }

  public void Initialize(Vector2 direction)
  {
    _rb.linearVelocity = direction.normalized * speed;
    Destroy(gameObject, 5f); // Destrói automaticamente após 5 segundos se não acertar nada
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    // Se atingir o Player
    if (collision.CompareTag("Player"))
    {
      PlayerController player = collision.GetComponent<PlayerController>();
      HealthSystem playerHealth = collision.GetComponent<HealthSystem>();

      if (playerHealth != null)
      {
        playerHealth.TakeDamage(damage, true, player);
      }
      Destroy(gameObject);
    }
    // Se atingir o chão (camada Ground)
    else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      Destroy(gameObject);
    }
  }
}