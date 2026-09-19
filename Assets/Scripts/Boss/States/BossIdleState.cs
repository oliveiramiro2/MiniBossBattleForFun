using UnityEngine;

public class BossIdleState : IBossState
{
  private BossController _boss;

  public BossIdleState(BossController boss)
  {
    _boss = boss;
  }

  public void Enter()
  {
    Debug.Log("[Boss FSM] Entrou no estado: IDLE (Aguardando/Patrulhando)");
    _boss.SpriteRenderer.color = Color.white; // Cor padrão
  }

  public void UpdateState()
  {
    // Comportamento de espera ou leve movimentação de patrulha se desejar
  }

  public void Exit() { }
}