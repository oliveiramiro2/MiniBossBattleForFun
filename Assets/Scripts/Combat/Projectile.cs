using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
  private float _damage = 15f;
  private Rigidbody2D _rb;
  private ProjectilePool _pool;
  private Coroutine _lifeRoutine;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
  }

  public void SetPool(ProjectilePool pool)
  {
    _pool = pool;
  }

  public void Initialize(Vector2 direction, float speed, float damage)
  {
    _damage = damage;
    _rb.linearVelocity = direction.normalized * speed;

    if (_lifeRoutine != null) StopCoroutine(_lifeRoutine);
    _lifeRoutine = StartCoroutine(LifeRoutine(5f)); // Retorna à pool após 5 segundos se não atingir nada
  }

  private IEnumerator LifeRoutine(float delay)
  {
    yield return new WaitForSeconds(delay);
    ReturnToPool();
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
      ReturnToPool();
    }
    else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      ReturnToPool();
    }
  }

  private void ReturnToPool()
  {
    if (_lifeRoutine != null) StopCoroutine(_lifeRoutine);
    _rb.linearVelocity = Vector2.zero;

    if (_pool != null)
    {
      _pool.ReturnToPool(this);
    }
    else
    {
      Destroy(gameObject); // Fallback caso não esteja usando pool
    }
  }
}