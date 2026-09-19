using UnityEngine;

public class BossRangedState : IBossState
{
  private BossController _boss;

  public BossRangedState(BossController boss)
  {
    _boss = boss;
  }

  public void Enter()
  {
    Debug.Log("[Boss FSM] Entrou no estado: MAGIA SOMBRIA (Projéteis em leque)");
    _boss.SpriteRenderer.color = Color.purple; // Fica roxo sinalizando magia
  }

  public void UpdateState()
  {
    // Lógica de disparo de projéteis à distância
  }

  public void Exit() { }
}