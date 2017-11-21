using System;
using Marbles.Systems.Contracts;
using UnityEngine;
using Zenject;

namespace Marbles.Systems
{
    public class CameraController : ICameraController, ITickable, IInitializable
    {
        private const float ZoomSpeed = 0.1f;

        public Camera Camera { get { return Camera.main; } }

        private Vector3 PositionalOffset { get; set; }
        private Transform target;
        private float StartingZoomAmount;
        private float zoomAmount;

        float nextZoomReset;
        bool needsReset;


        public Transform Target
        {
            get { return target; }
            set
            {
                target = value;
                PositionalOffset = Camera.transform.position - target.position;
            }
        }

        public void Tick()
        {
            FollowTarget();
            HandleZoomReset();
        }

        private void HandleZoomReset()
        {
            if (needsReset && Time.time > nextZoomReset)
            {
                zoomAmount = 0f;
                needsReset = false;
            }
        }

        private void FollowTarget()
        {
            if (target != null)
            {
                Vector3 position = Camera.transform.position;
                var offsetPosition = Target.position + PositionalOffset;

                position.x = offsetPosition.x;
                position.z = offsetPosition.z;
                position.y = Mathf.Lerp(position.y, StartingZoomAmount - zoomAmount, ZoomSpeed);

                Camera.transform.position = position;
            }
        }

        public void Initialize()
        {
            Refresh();

            zoomAmount = 0f;
        }

        public void Refresh()
        {
            StartingZoomAmount = Camera.transform.position.y;
        }

        public void Zoom(float amount)
        {
            zoomAmount = amount;
        }

        public void Zoom(float amount, float duration)
        {
            Zoom(amount);
            needsReset = true;
            nextZoomReset = Time.time + duration;
        }
    }
}
