using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
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

  [Header("Ground Check")]
  [SerializeField] private Transform groundCheck;
  [SerializeField] private float groundCheckRadius = 0.2f;
  [SerializeField] private LayerMask whatIsGround;
  private bool _isGrounded;

  private Rigidbody2D _rb;
  private SpriteRenderer _spriteRenderer;
  private int _facingDirection = 1; // 1 = Direita, -1 = Esquerda

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    // Verifica se está tocando no chão com base em um círculo invisível nos pés
    _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
  }

  public void Move(float direction)
  {
    if (_isDashing) return; // Trava o controle direcional durante o dash

    _rb.linearVelocity = new Vector2(direction * moveSpeed, _rb.linearVelocity.y);

    if (direction != 0)
    {
      _facingDirection = direction > 0 ? 1 : -1;
      _spriteRenderer.flipX = _facingDirection < 0; // Vira o sprite para a esquerda se necessário
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
    IsInvulnerable = true; // Ativa i-frames (Imunidade contra ataques do chefe)

    float originalGravity = _rb.gravityScale;
    _rb.gravityScale = 0f; // Zera a gravidade para o dash ser perfeitamente horizontal
    _rb.linearVelocity = new Vector2(_facingDirection * dashSpeed, 0f);

    // Feedback visual simples: deixa o player translúcido durante a invulnerabilidade
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
    Debug.Log("[Player] Ataque corpo a corpo acionado!");
  }

  // Desenha o círculo de detecção de solo na aba Scene da Unity
  private void OnDrawGizmosSelected()
  {
    if (groundCheck != null)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
  }
}