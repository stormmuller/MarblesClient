using Marbles.Components;
using Marbles.Systems.Contracts;
using UnityEngine;
using Marbles.Components.Levels;

namespace Marbles.Systems.Configurations
{
    public class ClickableConfiguration : IConfigurationHandler
    { 
        private readonly ISystemConfiguration Configuration;
        private readonly ILevelLoader levelLoader;

        public ClickableConfiguration(ILevelLoader levelLoader)
        {
            this.levelLoader = levelLoader;

            Configuration =
                new SystemConfiguration()
                .AddType<Clickable>()
                .Calls(c => LoadScene(c));
        }

        public void Handle(Component component)
        {
            Configuration.Handle(component);
        }


        private void LoadScene(Component component)
        {
            this.levelLoader.LoadLevel(component.GetComponent<Level>());
        }
    }
}
