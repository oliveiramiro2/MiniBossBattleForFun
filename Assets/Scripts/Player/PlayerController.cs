using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(HealthSystem))]
public class PlayerController : MonoBehaviour
{
  [Header("Movement Settings")]
  [SerializeField] private float moveSpeed = 8f;
  [SerializeField] private float jumpForce = 12f;

  [Header("Dash Settings")]
  [SerializeField] private float dashSpeed = 22f;
  [SerializeField] private float dashDuration = 0.15f;
  [SerializeField] private float dashCooldown = 0.7f;
  private bool _canDash = true;
  private bool _isDashing;
  public bool IsInvulnerable { get; private set; }

  [Header("Combat Settings")]
  [SerializeField] private Transform attackPoint;
  [SerializeField] private float attackRange = 1.2f;
  [SerializeField] private LayerMask bossLayer;
  [SerializeField] private float attackDamage = 25f;

  [Header("Ground Check")]
  [SerializeField] private Transform groundCheck;
  [SerializeField] private float groundCheckRadius = 0.2f;
  [SerializeField] private LayerMask whatIsGround;
  private bool _isGrounded;

  private Rigidbody2D _rb;
  private SpriteRenderer _spriteRenderer;
  private int _facingDirection = 1;
  private HealthSystem _healthSystem;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _healthSystem = GetComponent<HealthSystem>();
  }

  void Start()
  {
    // Inscreve no evento de morte do HealthSystem
    _healthSystem.OnDeath += Die;
  }

  private void Update()
  {
    _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
  }

  public void Move(float direction)
  {
    if (_isDashing) return;

    _rb.linearVelocity = new Vector2(direction * moveSpeed, _rb.linearVelocity.y);

    if (direction != 0)
    {
      _facingDirection = direction > 0 ? 1 : -1;
      _spriteRenderer.flipX = _facingDirection < 0;

      // Atualiza a posição do ponto de ataque de acordo com a direção que o player olha
      if (attackPoint != null)
      {
        attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x) * _facingDirection, attackPoint.localPosition.y, 0);
      }
    }
  }

  public void Jump()
  {
    if (_isDashing || !_isGrounded) return;

    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
  }

  public void Dash()
  {
    if (!_canDash || _isDashing) return;

    StartCoroutine(DashRoutine());
  }

  private IEnumerator DashRoutine()
  {
    _canDash = false;
    _isDashing = true;
    IsInvulnerable = true;

    float originalGravity = _rb.gravityScale;
    _rb.gravityScale = 0f;
    _rb.linearVelocity = new Vector2(_facingDirection * dashSpeed, 0f);

    Color originalColor = _spriteRenderer.color;
    _spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.4f);

    yield return new WaitForSeconds(dashDuration);

    _rb.gravityScale = originalGravity;
    _isDashing = false;
    IsInvulnerable = false;
    _spriteRenderer.color = originalColor;

    yield return new WaitForSeconds(dashCooldown);
    _canDash = true;
  }

  public void Attack()
  {
    if (attackPoint == null) return;

    // Detecta todos os inimigos (Boss) na área do ataque
    Collider2D[] hitBosses = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, bossLayer);
    foreach (Collider2D bossCol in hitBosses)
    {
      HealthSystem bossHealth = bossCol.GetComponent<HealthSystem>();
      if (bossHealth != null)
      {
        bossHealth.TakeDamage(attackDamage);
      }
    }
    Debug.Log("[Player] Ataque corpo a corpo executado!");
  }

  private void OnDestroy()
  {
    if (_healthSystem != null)
    {
      _healthSystem.OnDeath -= Die;
    }
  }

  private void Die()
  {
    Debug.Log("O Player foi derrotado!");
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected()
  {
    if (groundCheck != null)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    if (attackPoint != null)
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
  }
}