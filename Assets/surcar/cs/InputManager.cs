using UI_InputSystem.Base;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 MouseInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool AttackPressed { get; private set; }

    public bool ActionPressed { get; private set; }

    private void Update()
    {
        /*MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        JumpPressed = Input.GetButtonDown("Jump");
        AttackPressed = Input.GetButtonDown("Fire1");*/



        MoveInput = new Vector2(UIInputSystem.ME.GetAxisHorizontal(JoyStickAction.Movement), UIInputSystem.ME.GetAxisVertical(JoyStickAction.Movement));
        MouseInput = new Vector2(UIInputSystem.ME.GetAxisHorizontal(JoyStickAction.CameraLook), UIInputSystem.ME.GetAxisVertical(JoyStickAction.CameraLook));
        JumpPressed = UIInputSystem.ME.GetButton(ButtonAction.Jump);
        AttackPressed = UIInputSystem.ME.GetButton(ButtonAction.Attack) || UIInputSystem.ME.GetButton(ButtonAction.Attack2);
        ActionPressed = UIInputSystem.ME.GetButton(ButtonAction.Action);
    }
}