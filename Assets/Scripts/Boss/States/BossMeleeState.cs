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
    Debug.Log("[Boss FSM] Entrou no estado: ATAQUE CORPO A CORPO (Machado/Tridente)");
    _boss.SpriteRenderer.color = Color.red; // Fica vermelho sinalizando perigo iminente
  }

  public void UpdateState()
  {
    // Lógica de ataque corpo a corpo (ex: investida rápida ou golpe pesado)
  }

  public void Exit() { }
}