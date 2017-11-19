using UnityEngine;

namespace Marbles.UnityStyleComponents
{
    public class Follow : MonoBehaviour
    {
        public Transform followTarget;

        private Vector3 offset;

        // Use this for initialization
        void Start()
        {
            offset = transform.position - followTarget.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = followTarget.position + offset;
        }
    }
}
