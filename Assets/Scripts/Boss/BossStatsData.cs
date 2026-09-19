using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "Game/Boss Stats")]
public class BossStatsData : ScriptableObject
{
  [Header("Ranges")]
  public float meleeRange = 3f;
  public float rangedRange = 8f;

  [Header("Combat & Stats")]
  public float maxHealth = 200f;
  public float meleeDamage = 20f;
  public float meleeCooldown = 1.2f;
  public float projectileDamage = 15f;
  public float projectileSpeed = 12f;
  public float fireCooldown = 1.5f;
}