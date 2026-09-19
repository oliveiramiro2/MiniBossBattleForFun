using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(HealthSystem))]
public class BossController : MonoBehaviour
{
  [Header("Data Asset")]
  [SerializeField] private BossStatsData stats;
  public BossStatsData Stats => stats;

  [Header("Target & References")]
  [SerializeField] private Transform playerTransform;
  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private Transform firePoint;

  // Estados da FSM
  private IBossState _currentState;
  public BossIdleState IdleState { get; private set; }
  public BossMeleeState MeleeState { get; private set; }
  public BossRangedState RangedState { get; private set; }

  public Rigidbody2D Rb { get; private set; }
  public SpriteRenderer SpriteRenderer { get; private set; }
  public Transform PlayerTransform => playerTransform;
  private HealthSystem _healthSystem;

  private float _meleeTimer;

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
    if (playerTransform == null || stats == null) return;

    float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

    // Transições baseadas nos ranges definidos no ScriptableObject
    if (distanceToPlayer <= stats.meleeRange)
    {
      if (_currentState != MeleeState) SwitchState(MeleeState);
    }
    else if (distanceToPlayer <= stats.rangedRange)
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
    if (_meleeTimer > 0 || playerTransform == null || stats == null) return;
    _meleeTimer = stats.meleeCooldown;

    float distance = Vector2.Distance(transform.position, playerTransform.position);
    if (distance <= stats.meleeRange + 0.5f)
    {
      HealthSystem playerHealth = playerTransform.GetComponent<HealthSystem>();
      PlayerController playerCtrl = playerTransform.GetComponent<PlayerController>();
      if (playerHealth != null)
      {
        playerHealth.TakeDamage(stats.meleeDamage, true, playerCtrl);
        Debug.Log("[Boss] Ataque corpo a corpo acertou o Jogador!");
      }
    }
  }

  public void ShootProjectile()
  {
    if (projectilePrefab == null || playerTransform == null || stats == null) return;

    GameObject projObj = Instantiate(projectilePrefab, firePoint != null ? firePoint.position : transform.position, Quaternion.identity);
    Projectile proj = projObj.GetComponent<Projectile>();
    if (proj != null)
    {
      Vector2 direction = (playerTransform.position - transform.position).normalized;
      proj.Initialize(direction, stats.projectileSpeed, stats.projectileDamage);
    }
  }

  private void Die()
  {
    Debug.Log("[Boss] O Chefe foi derrotado por completo!");
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected()
  {
    if (stats == null) return;
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, stats.meleeRange);
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, stats.rangedRange);
  }
}