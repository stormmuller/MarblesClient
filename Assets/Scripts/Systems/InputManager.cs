using Marbles.Enums;
using Marbles.Systems.Contracts;
using UnityEngine;

namespace Marbles.Systems
{
    public class InputManager : IInputManager
    {
        public bool LeftMouseButtonDown { get { return Input.GetMouseButtonDown((int)MouseButtons.Left); } }
        public bool LeftMouseButtonUp { get { return Input.GetMouseButtonUp((int)MouseButtons.Left); } }

        public Vector3 MousePosition { get { return Input.mousePosition; } }

    }
}
