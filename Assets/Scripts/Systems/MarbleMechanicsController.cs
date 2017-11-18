using System;
using Marbles.Enums;
using Marbles.Systems.Contracts;
using UnityEngine;
using Zenject;

namespace Marbles.Systems
{
    public class MarbleMechanicsController : IMarbleMechanicsController, ITickable
    {
        private const string PlaneRayCastLayerName = "Battle Ground Plane";
        private const float scrollSpeed = 0.5f; 

        private readonly IInputManager inputManager;

        private MarbleShotStatus marbleShotStatus;
        private LineRenderer lineRenderer;
        private LayerMask layerMask;

        public MarbleMechanicsController(IInputManager inputManager)
        {
            this.inputManager = inputManager;

            marbleShotStatus = MarbleShotStatus.Idle;
            layerMask = 1 << LayerMask.NameToLayer(PlaneRayCastLayerName);
        }

        public void PrepareMarbleForShot(Component component)
        {
            marbleShotStatus = MarbleShotStatus.PreparingShot;

            lineRenderer = component.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
        }

        public void EndMarbleShot(Component component)
        {
            marbleShotStatus = MarbleShotStatus.Idle;

            lineRenderer = component.GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
        }

        public void Tick()
        {
            if (marbleShotStatus == MarbleShotStatus.PreparingShot)
            {
                var ray = Camera.main.ScreenPointToRay(inputManager.MousePosition);
                RaycastHit raycastHit;

                lineRenderer.SetPosition(0, lineRenderer.transform.position);

                ScrollLineRenderer();

                if (Physics.Raycast(ray, out raycastHit, 1000f, layerMask))
                {
                    lineRenderer.SetPosition(1, raycastHit.point);
                }
            }
        }

        private void ScrollLineRenderer()
        {
            var currentScrollPos = lineRenderer.material.GetTextureOffset("_MainTex");

            currentScrollPos.x = currentScrollPos.x + scrollSpeed * Time.deltaTime;
            lineRenderer.material.SetTextureOffset("_MainTex", currentScrollPos);
        }
    }
}
