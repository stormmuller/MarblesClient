using Marbles.Components;
using Marbles.Systems.Contracts;
using Marbles.Systems.HoverableConfiguration;
using System.Collections.Generic;

namespace Marbles.Systems
{
    public class HoverableController : IHoverableController
    {
        private readonly List<IHoverableConfiguration> hoverableConfigurations;
        
        public HoverableController(List<IHoverableConfiguration> hoverableConfigurations)
        {
            this.hoverableConfigurations = hoverableConfigurations;
        }

        public void HandleHoverFor(Hoverable hoverable)
        {
            foreach (var hoverableConfiguration in hoverableConfigurations)
            {
                if (hoverableConfiguration.IsEntityCompliant(hoverable))
                {
                    hoverableConfiguration.OnHover(hoverable);
                }
            }
        }
        
        public void HandleBlurFor(Hoverable hoverable)
        {
            foreach (var hoverableConfiguration in hoverableConfigurations)
            {
                if (hoverableConfiguration.IsEntityCompliant(hoverable))
                {
                    hoverableConfiguration.OnBlur(hoverable);
                }
            }
        }
    }
}
