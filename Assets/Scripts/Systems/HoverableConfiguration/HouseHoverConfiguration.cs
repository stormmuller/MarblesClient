using Marbles.Components;
using UnityEngine;

namespace Marbles.Systems.HoverableConfiguration
{
    public class HouseHoverConfiguration : IHoverableConfiguration
    {
        const float Frequency = 4f;
        const float YShift = 1.5f;
        const float Stretch = 0.50f;
        const float Squash = 0.4f;
        
        private readonly Color HoverColor;

        public HouseHoverConfiguration()
        {
            HoverColor = new Color(0.5f, 0.5f, 0.7f);
        }

        public void OnHover(Hoverable hoverable)
        {
            var sinValue = (Mathf.Sin(Time.time * Frequency) * Stretch + YShift) * Squash;

            hoverable
                .GetComponent<Renderer>()
                .material
                .SetColor("_EmissionColor", Color.Lerp(Color.black, HoverColor, sinValue));
        }

        public bool IsCompliant(Hoverable hoverable)
        {
            return hoverable.GetComponent<House>() != null;
        }

        public void OnBlur(Hoverable hoverable)
        {
            hoverable
                .GetComponent<Renderer>()
                .material.SetColor("_EmissionColor", Color.black);
        }

        public bool IsEntityCompliant(Hoverable component)
        {
            return component.GetComponent<House>() != null;
        }

        public bool IsEntityOrParentCompliant(Hoverable component)
        {
            return 
                component.GetComponentInParent<House>() != null || component.GetComponent<House>() != null;
        }
    }
}
