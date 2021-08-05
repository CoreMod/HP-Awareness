using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace HPAware
{
    
    [Label("Configuration")]
    public class Modconfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Disable Moon Lord Shader")]
        [Tooltip("Moon Lord's shader causes the overlays to be invisible. Turn this on to remove it.")]
        public bool DisableMLShader;

        [Header("[i:29] Hurt Overlay")]

        [Label("Disable Overlay for Getting Hit")]
        public bool DisableHurtOverlay;

        [Label("Use Classic Hurt Overlay")]
        public bool ClassicHurtOverlay;

        [Label("Have variable intensity")]
        [Tooltip("Hurt overlay fluctuates in intensity depending on how often you get hurt.")]
        public bool HaveIntensity;

        [Header("[i:29] Low HP Overlay")]

        [Label("Disable overlay for low health")]
        public bool DisableLowHpOverlay;

        [Label("Use Classic Low HP Overlay")]
        public bool ClassicLowHpOverlay;

        [Label("Low HP Percentage")]
        [Tooltip("Having HP lower or equal to this percentage activates the overlay.")]
        [DefaultValue(0.25)]
        public float Overlaytrigger;

        [Header("[i:28] Potion Pop Up")]

        [Label("Disable Potion Sickness Audio")]
        [Tooltip("When enabled, plays a higher-pitched potion-drinking sound effect when potion sickness disappears.")]
        public bool DisablePSAudio;

        [Label("Disable Potion Sickness Visual")]
        [Tooltip("When enabled, an icon will appear above the player when potion sickness disappears.")]
        public bool DisablePSVisual;

        [Header("[i:2701] Debuff Pop Up")]

        [Label("Disable Buff Pop Ups")]
        public bool DisableBuffVisual;
    }
}
