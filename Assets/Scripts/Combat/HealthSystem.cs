using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
  [SerializeField] private float maxHealth = 100f;
  public float CurrentHealth { get; private set; }

  public event Action<float> OnHealthChanged; // Disparado quando a vida muda (percentual de 0 a 1)
  public event Action OnDeath; // Disparado quando a vida chega a zero

  private void Awake()
  {
    CurrentHealth = maxHealth;
  }

  public void TakeDamage(float amount, bool isPlayer = false, PlayerController player = null)
  {
    // Se for o player e ele estiver no meio de um Dash, ignora o dano (I-Frames!)
    if (isPlayer && player != null && player.IsInvulnerable)
    {
      Debug.Log("[Combat] Dano evitado pelo Dash (I-Frame)!");
      return;
    }

    CurrentHealth -= amount;
    CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

    // Atualiza percentual para interfaces futuras
    OnHealthChanged?.Invoke(CurrentHealth / maxHealth);
    Debug.Log($"[{gameObject.name}] Sofreu {amount} de dano. Vida atual: {CurrentHealth}/{maxHealth}");

    if (CurrentHealth <= 0f)
    {
      OnDeath?.Invoke();
    }
  }
}