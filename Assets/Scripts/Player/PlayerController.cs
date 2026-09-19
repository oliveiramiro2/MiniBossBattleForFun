using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(HealthSystem))]
public class PlayerController : MonoBehaviour
{
  [Header("Data Asset")]
  [SerializeField] private PlayerStatsData stats;

  [Header("Combat References")]
  [SerializeField] private Transform attackPoint;
  [SerializeField] private LayerMask bossLayer;

  [Header("Ground Check")]
  [SerializeField] private Transform groundCheck;
  [SerializeField] private float groundCheckRadius = 0.2f;
  [SerializeField] private LayerMask whatIsGround;
  private bool _isGrounded;

  private Rigidbody2D _rb;
  private SpriteRenderer _spriteRenderer;
  private int _facingDirection = 1;

  private bool _canDash = true;
  private bool _isDashing;
  public bool IsInvulnerable { get; private set; }

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
  }

  public void Move(float direction)
  {
    if (_isDashing) return;

    _rb.linearVelocity = new Vector2(direction * stats.moveSpeed, _rb.linearVelocity.y);

    if (direction != 0)
    {
      _facingDirection = direction > 0 ? 1 : -1;
      _spriteRenderer.flipX = _facingDirection < 0;

      if (attackPoint != null)
      {
        attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x) * _facingDirection, attackPoint.localPosition.y, 0);
      }
    }
  }

  public void Jump()
  {
    if (_isDashing || !_isGrounded) return;

    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, stats.jumpForce);
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
    _rb.linearVelocity = new Vector2(_facingDirection * stats.dashSpeed, 0f);

    Color originalColor = _spriteRenderer.color;
    _spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.4f);

    yield return new WaitForSeconds(stats.dashDuration);

    _rb.gravityScale = originalGravity;
    _isDashing = false;
    IsInvulnerable = false;
    _spriteRenderer.color = originalColor;

    yield return new WaitForSeconds(stats.dashCooldown);
    _canDash = true;
  }

  public void Attack()
  {
    if (attackPoint == null) return;

    Collider2D[] hitBosses = Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, bossLayer);
    foreach (Collider2D bossCol in hitBosses)
    {
      HealthSystem bossHealth = bossCol.GetComponent<HealthSystem>();
      if (bossHealth != null)
      {
        bossHealth.TakeDamage(stats.attackDamage);
      }
    }
    Debug.Log("[Player] Ataque executado!");
  }

  private void OnDrawGizmosSelected()
  {
    if (groundCheck != null)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    if (attackPoint != null && stats != null)
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(attackPoint.position, stats.attackRange);
    }
  }
}