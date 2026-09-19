using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStatsData : ScriptableObject
{
  [Header("Movement")]
  public float moveSpeed = 8f;
  public float jumpForce = 12f;

  [Header("Dash")]
  public float dashSpeed = 22f;
  public float dashDuration = 0.15f;
  public float dashCooldown = 0.7f;

  [Header("Combat")]
  public float maxHealth = 100f;
  public float attackDamage = 25f;
  public float attackRange = 1.2f;
}