using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(HealthSystem))]
public class BossController : MonoBehaviour
{
  [Header("Target & Ranges")]
  [SerializeField] private Transform playerTransform;
  [SerializeField] private float meleeRange = 3f;
  [SerializeField] private float rangedRange = 8f;

  [Header("Combat & Projectiles")]
  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private Transform firePoint;
  [SerializeField] private float meleeDamage = 20f;
  private float _meleeCooldown = 1.2f;
  private float _meleeTimer;

  // Estados
  private IBossState _currentState;
  public BossIdleState IdleState { get; private set; }
  public BossMeleeState MeleeState { get; private set; }
  public BossRangedState RangedState { get; private set; }

  public Rigidbody2D Rb { get; private set; }
  public SpriteRenderer SpriteRenderer { get; private set; }
  public Transform PlayerTransform => playerTransform;
  private HealthSystem _healthSystem;

  private void Awake()
  {
    Rb = GetComponent<Rigidbody2D>();
    SpriteRenderer = GetComponent<SpriteRenderer>();
    _healthSystem = GetComponent<HealthSystem>();

    // Inicializa os estados
    IdleState = new BossIdleState(this);
    MeleeState = new BossMeleeState(this);
    RangedState = new BossRangedState(this);
  }

  private void Start()
  {
    if (playerTransform == null)
    {
      GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
      if (playerObj != null) playerTransform = playerObj.transform;
    }

    // Inscreve no evento de morte do HealthSystem
    _healthSystem.OnDeath += Die;

    SwitchState(IdleState);
  }

  private void OnDestroy()
  {
    if (_healthSystem != null)
    {
      _healthSystem.OnDeath -= Die;
    }
  }

  private void Update()
  {
    if (_currentState != null)
    {
      _currentState.UpdateState();
      EvaluateStateTransitions();
    }

    if (_meleeTimer > 0) _meleeTimer -= Time.deltaTime;
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

  public void PerformMeleeAttack()
  {
    if (_meleeTimer > 0 || playerTransform == null) return;
    _meleeTimer = _meleeCooldown;

    float distance = Vector2.Distance(transform.position, playerTransform.position);
    if (distance <= meleeRange + 0.5f)
    {
      HealthSystem playerHealth = playerTransform.GetComponent<HealthSystem>();
      PlayerController playerCtrl = playerTransform.GetComponent<PlayerController>();
      if (playerHealth != null)
      {
        playerHealth.TakeDamage(meleeDamage, true, playerCtrl);
        Debug.Log("[Boss] Ataque corpo a corpo acertou o Jogador!");
      }
    }
  }

  public void ShootProjectile()
  {
    if (projectilePrefab == null || playerTransform == null) return;

    GameObject projObj = Instantiate(projectilePrefab, firePoint != null ? firePoint.position : transform.position, Quaternion.identity);
    Projectile proj = projObj.GetComponent<Projectile>();
    if (proj != null)
    {
      Vector2 direction = (playerTransform.position - transform.position).normalized;
      proj.Initialize(direction);
    }
  }

  private void Die()
  {
    Debug.Log("[Boss] O Chefe foi derrotado por completo!");
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, meleeRange);
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, rangedRange);
  }
}