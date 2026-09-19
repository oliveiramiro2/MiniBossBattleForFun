using UnityEngine;

public class BossRangedState : IBossState
{
  private BossController _boss;
  private float _fireTimer;

  public BossRangedState(BossController boss)
  {
    _boss = boss;
  }

  public void Enter()
  {
    Debug.Log("[Boss FSM] Entrou no estado: MAGIA SOMBRIA (Disparando projéteis)");
    _boss.SpriteRenderer.color = Color.magenta;
    _fireTimer = 0f;
  }

  public void UpdateState()
  {
    if (_boss.Stats == null) return;

    _fireTimer += Time.deltaTime;
    if (_fireTimer >= _boss.Stats.fireCooldown)
    {
      _fireTimer = 0f;
      _boss.ShootProjectile();
    }
  }

  public void Exit() { }
}