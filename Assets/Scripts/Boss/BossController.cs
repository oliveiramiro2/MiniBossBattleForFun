using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BossController : MonoBehaviour
{
  [Header("Target & Ranges")]
  [SerializeField] private Transform playerTransform;
  [SerializeField] private float meleeRange = 3f;
  [SerializeField] private float rangedRange = 8f;

  [Header("Stats")]
  [SerializeField] private float maxHealth = 200f;
  public float CurrentHealth { get; private set; }

  // Estados
  private IBossState _currentState;
  public BossIdleState IdleState { get; private set; }
  public BossMeleeState MeleeState { get; private set; }
  public BossRangedState RangedState { get; private set; }

  public Rigidbody2D Rb { get; private set; }
  public SpriteRenderer SpriteRenderer { get; private set; }
  public Transform PlayerTransform => playerTransform;

  private void Awake()
  {
    Rb = GetComponent<Rigidbody2D>();
    SpriteRenderer = GetComponent<SpriteRenderer>();
    CurrentHealth = maxHealth;

    // Inicializa os estados
    IdleState = new BossIdleState(this);
    MeleeState = new BossMeleeState(this);
    RangedState = new BossRangedState(this);
  }

  private void Start()
  {
    // Se o player não foi arrastado no Inspector, tenta achar pela Tag
    if (playerTransform == null)
    {
      GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
      if (playerObj != null) playerTransform = playerObj.transform;
    }

    // Começa no estado Idle
    SwitchState(IdleState);
  }

  private void Update()
  {
    if (_currentState != null)
    {
      _currentState.UpdateState();
      EvaluateStateTransitions();
    }
  }

  public void SwitchState(IBossState newState)
  {
    if (_currentState != null) _currentState.Exit();
    _currentState = newState;
    _currentState.Enter();
  }

  private void EvaluateStateTransitions()
  {
    if (playerTransform == null) return;

    float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

    // Árvore de Decisão por Distância
    if (distanceToPlayer <= meleeRange)
    {
      if (_currentState != MeleeState) SwitchState(MeleeState);
    }
    else if (distanceToPlayer <= rangedRange)
    {
      if (_currentState != RangedState) SwitchState(RangedState);
    }
    else
    {
      if (_currentState != IdleState) SwitchState(IdleState);
    }
  }

  // Método para receber dano (será usado no Passo 4)
  public void TakeDamage(float amount)
  {
    CurrentHealth -= amount;
    Debug.Log($"[Boss] Sofreu {amount} de dano! Vida restante: {CurrentHealth}");
    if (CurrentHealth <= 0)
    {
      Die();
    }
  }

  private void Die()
  {
    Debug.Log("[Boss] O Chefe foi derrotado!");
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected()
  {
    // Desenha os raios de alcance na Unity Scene
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, meleeRange);
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, rangedRange);
  }
}