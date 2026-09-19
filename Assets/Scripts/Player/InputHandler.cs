using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerController))]
public class InputHandler : MonoBehaviour
{
  private PlayerController _player;

  private void Awake()
  {
    _player = GetComponent<PlayerController>();
  }

  private void Update()
  {
    if (_player == null || Keyboard.current == null) return;

    // Leitura de Movimento Horizontal (A/D ou Setas)
    float moveInput = 0f;
    if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;
    if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;

    if (moveInput != 0)
    {
      ICommand moveCmd = new MoveCommand(moveInput);
      moveCmd.Execute(_player);
    }

    // Pulo (Espaço)
    if (Keyboard.current.spaceKey.wasPressedThisFrame)
    {
      ICommand jumpCmd = new JumpCommand();
      jumpCmd.Execute(_player);
    }

    // Dash (Left Shift)
    if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
    {
      ICommand dashCmd = new DashCommand();
      dashCmd.Execute(_player);
    }

    // Ataque (Tecla J ou Botão esquerdo do mouse)
    bool attackPressed = Keyboard.current.jKey.wasPressedThisFrame ||
                         (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);
    if (attackPressed)
    {
      ICommand attackCmd = new AttackCommand();
      attackCmd.Execute(_player);
    }
  }
}