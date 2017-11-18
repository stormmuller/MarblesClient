using Contracts.Systems.Contracts;
using Marbles.Components;
using Marbles.Systems.Configurations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UnityEngine.EventSystems;
using Marbles.Systems.Contracts;

namespace Marbles.Systems
{
    public class ClickableController : IClickableController, IInitializable, ITickable
    {
        private readonly MouseDownConfiguration mouseDownConfiguration;
        private readonly MouseUpConfiguration mouseUpConfiguration;
        private readonly IInputManager inputManager;

        private Component lastComponentOnMouseDown;

        public ClickableController(MouseDownConfiguration clickableConfiguration, MouseUpConfiguration mouseUpConfiguration, IInputManager inputManager)
        {
            this.mouseDownConfiguration = clickableConfiguration;
            this.mouseUpConfiguration = mouseUpConfiguration;
            this.inputManager = inputManager;
        }

        public void Initialize()
        {
            var allButtonsInScene = Object.FindObjectsOfType<Button>();

            foreach (var button in allButtonsInScene)
            {
                button.onClick.AddListener(() => InvokeClickOnButton(button));
            }
        }

        public void Tick()
        {
            InvokeNonUiClickables();
            InvokeMouseUp();
        }

        private void InvokeNonUiClickables()
        {
            Ray raycast;
            RaycastHit rayHit;

            raycast = Camera.main.ScreenPointToRay(inputManager.MousePosition);

            if (Physics.Raycast(raycast, out rayHit))
            {
                var clickable = GetValidNonUiClickable(rayHit.collider);

                if (clickable == null)
                {
                    return;
                }

                if (inputManager.LeftMouseButtonDown)
                {
                    mouseDownConfiguration.Handle(clickable);
                }
            }
        }

        private Clickable GetValidNonUiClickable(Component component)
        {
            var clickable = component.GetComponent<Clickable>();

            if (clickable == null || clickable.GetComponent<UIBehaviour>() != null)
            {
                return null;
            }

            lastComponentOnMouseDown = component;
            return clickable;
        }

        private void InvokeClickOnButton(Button button)
        {
            mouseDownConfiguration.Handle(button);
            lastComponentOnMouseDown = button;
        }


        private void InvokeMouseUp()
        {
            if (lastComponentOnMouseDown != null && inputManager.LeftMouseButtonUp)
            {
                mouseUpConfiguration.Handle(lastComponentOnMouseDown);

                lastComponentOnMouseDown = null;
            }
        }
    }
}
