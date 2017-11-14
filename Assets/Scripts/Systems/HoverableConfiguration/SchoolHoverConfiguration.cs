using Marbles.Components;
using UnityEngine;

namespace Marbles.Systems.HoverableConfiguration
{
    public class SchoolHoverConfiguration : IHoverableConfiguration
    {

        const float Frequency = 4f;
        const float YShift = 1.5f;
        const float Stretch = 0.50f;
        const float Squash = 0.4f;

        private readonly Color HoverColor;

        public SchoolHoverConfiguration()
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

        public void OnBlur(Hoverable hoverable)
        {
            hoverable
                .GetComponent<Renderer>()
                .material.SetColor("_EmissionColor", Color.black);
        }

        public bool IsEntityCompliant(Hoverable component)
        {
            return component.GetComponent<School>() != null;
        }

        public bool IsEntityOrParentCompliant(Hoverable component)
        {
            return
                component.GetComponentInParent<School>() != null || component.GetComponent<School>() != null;
        }
    }
}
