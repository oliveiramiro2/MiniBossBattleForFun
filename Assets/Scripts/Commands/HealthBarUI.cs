using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
  [SerializeField] private HealthSystem healthSystem;
  [SerializeField] private Slider healthSlider;

  private void Start()
  {
    if (healthSystem != null && healthSlider != null)
    {
      healthSystem.OnHealthChanged += UpdateHealthBar;
      healthSlider.value = 1f; // Começa cheia (100%)
    }
  }

  private void OnDestroy()
  {
    if (healthSystem != null)
    {
      healthSystem.OnHealthChanged -= UpdateHealthBar;
    }
  }

  private void UpdateHealthBar(float healthNormalized)
  {
    if (healthSlider != null)
    {
      healthSlider.value = healthNormalized;
    }
    if (healthSlider.value <= 0)
    {
      healthSlider.fillRect.localScale = new Vector3(0f, 0f, 0f);
    }
  }
}