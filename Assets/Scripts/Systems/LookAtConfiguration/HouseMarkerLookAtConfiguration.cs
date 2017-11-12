using Marbles.Components;
using UnityEngine;

namespace Marbles.Systems.LookAtConfiguration
{
    public class HouseMarkerLookAtConfiguration : ILookAtConfiguration
    {
        public bool IsEntityCompliant(LookAt component)
        {
            return component.GetComponent<House>() != null;
        }

        public bool IsEntityOrParentCompliant(LookAt component)
        {
            return
                component.GetComponentInParent<House>() != null || component.GetComponent<House>() != null;
        }

        public void PerformLookUp(LookAt lookAt)
        {
            lookAt.transform.rotation = Quaternion.LookRotation(lookAt.transform.position - Camera.main.transform.position);
        }
    }
}
