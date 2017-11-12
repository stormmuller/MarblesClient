using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Marbles.ExtentionMethods;
using Marbles.Components;
using Marbles.Systems.Contracts;

namespace Marbles.Systems
{
    public class MousePointerController : ITickable
    {
        private IInputManager inputManager;
        private IHoverableController hoverableController;

        private HashSet<Hoverable> hoveredObjectsPreviousFrame = new HashSet<Hoverable>();
        private HashSet<Hoverable> hoveredObjectsThisFrame = new HashSet<Hoverable>();
        private HashSet<Hoverable> hoveredObjectsDelta = new HashSet<Hoverable>();

        [Inject]
        public void Construct(IInputManager inputManager, IHoverableController hoverableController)
        {
            this.inputManager = inputManager;
            this.hoverableController = hoverableController;
        }

        public void Tick()
        {
            var ray = Camera.main.ScreenPointToRay(inputManager.MousePosition);

            hoveredObjectsThisFrame.Clear();

            CheckForHover(ray);
            CheckForClick(ray);
            CheckForBlur(hoveredObjectsDelta);

            hoveredObjectsDelta.Clear();
            hoveredObjectsDelta.AddRange(hoveredObjectsPreviousFrame);
            hoveredObjectsDelta.ExceptWith(hoveredObjectsThisFrame);
            hoveredObjectsPreviousFrame.Clear();
            hoveredObjectsPreviousFrame.AddRange(hoveredObjectsThisFrame);
        }

        private void CheckForClick(Ray ray)
        {
            if (inputManager.LeftMouseButtonPress)
            {
                RaycastHit rayHitDetails;

                if (Physics.Raycast(ray, out rayHitDetails))
                {
                    // rayHitDetails.transform.GetComponent<Clickable>().OnClick();
                }
            }
        }

        private void CheckForHover(Ray ray)
        {
            RaycastHit rayHitDetails;

            if (Physics.Raycast(ray, out rayHitDetails))
            {
                var hoverable = rayHitDetails.transform.GetComponent<Hoverable>();

                if (hoverable != null)
                {
                    hoveredObjectsThisFrame.Add(hoverable);
                    hoverableController.HandleHoverFor(hoverable);
                }
            }
        }

        private void CheckForBlur(HashSet<Hoverable> hoveredObjectsDelta)
        {
            foreach (var hoveredObject in hoveredObjectsDelta)
            {
                hoverableController.HandleBlurFor(hoveredObject);
            }
        }
    }
}