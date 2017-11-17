using Contracts.Systems.Contracts;
using Marbles.Systems.Configurations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Marbles.Systems
{
    public class ClickableController : IClickableController, IInitializable
    {
        private readonly ClickableConfiguration clickableConfiguration;

        public ClickableController(ClickableConfiguration clickableConfiguration)
        {
            this.clickableConfiguration = clickableConfiguration;
        }

        public void Initialize()
        {
            var allButtonsInScene = Object.FindObjectsOfType<Button>();

            foreach (var button in allButtonsInScene)
            {
                button.onClick.AddListener(() => InvokeClickOnButton(button));
            }
        }

        private void InvokeClickOnButton(Button button)
        {
            clickableConfiguration.Handle(button);
        }
    }
}
