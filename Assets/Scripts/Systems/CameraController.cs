using Marbles.Systems.Contracts;
using UnityEngine;
using Zenject;

namespace Marbles.Systems
{
    public class CameraController : ICameraController, ITickable, IInitializable
    {
        private const float ZoomSpeed = 0.1f;

        public Camera Camera { get { return Camera.main; } }
        public float ZoomAmount { get; set; }

        private Vector3 PositionalOffset { get; set; }
        private Transform target;
        private float StartingZoomAmount;


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
        }

        private void FollowTarget()
        {
            if (target != null)
            {
                Vector3 position = Camera.transform.position;
                var offsetPosition = Target.position + PositionalOffset;

                position.x = offsetPosition.x;
                position.z = offsetPosition.z;
                position.y = Mathf.Lerp(position.y, StartingZoomAmount - ZoomAmount, ZoomSpeed);

                Camera.transform.position = position;
            }
        }

        public void Initialize()
        {
            Refresh();

            ZoomAmount = 0f;
        }

        public void Refresh()
        {
            StartingZoomAmount = Camera.transform.position.y;
        }
    }
}
