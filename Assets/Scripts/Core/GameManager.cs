using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
  [Header("Health References")]
  [SerializeField] private HealthSystem playerHealth;
  [SerializeField] private HealthSystem bossHealth;

  [Header("UI Panels (Opcional)")]
  [SerializeField] private GameObject victoryPanel;
  [SerializeField] private GameObject defeatPanel;

  private bool _isGameOver = false;

  private void Start()
  {
    // Garante que os painéis comecem desativados
    if (victoryPanel != null) victoryPanel.SetActive(false);
    if (defeatPanel != null) defeatPanel.SetActive(false);

    // Se as referências não foram arrastadas no Inspector, tenta encontrá-las na cena
    if (playerHealth == null)
    {
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if (player != null) playerHealth = player.GetComponent<HealthSystem>();
    }

    if (bossHealth == null)
    {
      BossController boss = FindAnyObjectByType<BossController>();
      if (boss != null) bossHealth = boss.GetComponent<HealthSystem>();
    }

    // Inscreve nos eventos de morte
    if (playerHealth != null)
    {
      playerHealth.OnDeath += HandlePlayerDeath;
    }

    if (bossHealth != null)
    {
      bossHealth.OnDeath += HandleBossDeath;
    }
  }

  private void OnDestroy()
  {
    // Desinscreve dos eventos para evitar vazamento de memória
    if (playerHealth != null)
    {
      playerHealth.OnDeath -= HandlePlayerDeath;
    }

    if (bossHealth != null)
    {
      bossHealth.OnDeath -= HandleBossDeath;
    }
  }

  private void Update()
  {
    // Atalho rápido para reiniciar o jogo apertando a tecla 'R' após o fim de jogo
    bool rPressed = Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame;
    if (_isGameOver && (rPressed || Input.GetKeyDown(KeyCode.R)))
    {
      RestartGame();
    }
  }

  private void HandlePlayerDeath()
  {
    if (_isGameOver) return;
    _isGameOver = true;

    Debug.Log("[GameManager] Jogador derrotado! Exibindo tela de Game Over.");
    if (defeatPanel != null) defeatPanel.SetActive(true);

    Time.timeScale = 0f; // Pausa o tempo do jogo
  }

  private void HandleBossDeath()
  {
    if (_isGameOver) return;
    _isGameOver = true;

    Debug.Log("[GameManager] Chefe derrotado! Vitória do Jogador!");
    if (victoryPanel != null) victoryPanel.SetActive(true);

    Time.timeScale = 0f; // Pausa o tempo do jogo
  }

  public void RestartGame()
  {
    Time.timeScale = 1f; // Restaura o tempo normal antes de recarregar a cena
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}