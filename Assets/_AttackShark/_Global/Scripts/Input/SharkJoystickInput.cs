using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkJoystickInput : MonoBehaviour, ISharkMoveInput
{
    [SerializeField] private Joystick _joystick;

    public Vector2 GetFrameInput()
    {
        return new Vector2(_joystick.Horizontal, _joystick.Vertical);
    }
}
