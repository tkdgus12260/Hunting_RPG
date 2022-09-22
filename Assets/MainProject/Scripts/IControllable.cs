using UnityEngine;

public interface IControllable
{
    Vector3 KeyboardInputDir { set; }

    Vector2 MouseDelta { set; }

    public delegate void InputActionDelegate();

    public InputActionDelegate OnAttack { get; set; }
    public InputActionDelegate OnRolling { get; set; }
    public InputActionDelegate OnJump { get; set; }
    public InputActionDelegate OnInventory { get; set; }
    public InputActionDelegate OnPause { get; set; }
}
