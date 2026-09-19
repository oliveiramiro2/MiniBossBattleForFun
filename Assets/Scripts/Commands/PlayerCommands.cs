using UnityEngine;

public class MoveCommand : ICommand
{
  private float _direction;

  public MoveCommand(float direction)
  {
    _direction = direction;
  }

  public void Execute(PlayerController player)
  {
    player.Move(_direction);
  }
}

public class JumpCommand : ICommand
{
  public void Execute(PlayerController player)
  {
    player.Jump();
  }
}

public class DashCommand : ICommand
{
  public void Execute(PlayerController player)
  {
    player.Dash();
  }
}

public class AttackCommand : ICommand
{
  public void Execute(PlayerController player)
  {
    player.Attack();
  }
}