using UnityEngine;

public class BossMeleeState : IBossState
{
  private BossController _boss;

  public BossMeleeState(BossController boss)
  {
    _boss = boss;
  }

  public void Enter()
  {
    Debug.Log("[Boss FSM] Entrou no estado: ATAQUE CORPO A CORPO");
    _boss.SpriteRenderer.color = Color.red;
  }

  public void UpdateState()
  {
    _boss.PerformMeleeAttack();
  }

  public void Exit() { }
}