using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace HPAware
{
    
    [Label("Configuration")]
    public class Modconfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Disable Moon Lord shader")]
        [Tooltip("Moon Lord's shader causes the overlays to be invisible. Turn this on to remove it.")]
        public bool DisableMLShader;

        [Header("[i:29] Hurt Overlay")]

        [Label("Disable overlay for getting hit")]
        public bool DisableHurtOverlay;

        [Label("Overlay to use")]
        [OptionStrings(new string[] { "HPOverlay", "NewHPOverlay", "HPOverlayFlat" })]
        [DefaultValue("NewHPOverlay")]
        public string HurtOverlayType;

        [Label("Have variable intensity")]
        [Tooltip("Hurt overlay fluctuates in intensity depending on how often you get hurt.")]
        public bool HaveIntensity;

        [Header("[i:29] Low HP Overlay")]

        [Label("Disable overlay for low health")]
        public bool DisableLowHpOverlay;

        [Label("Use classic low HP overlay")]
        public bool ClassicLowHpOverlay;

        [Label("Low HP Percentage")]
        [Tooltip("Having HP lower or equal to this percentage activates the overlay.")]
        [DefaultValue(0.25)]
        public float Overlaytrigger;

        [Header("[i:705] Player HP Bar")]

        [Label("Disable player HP bar")]
        public bool DisableHPBar;

        [Header("[i:28] Potion Pop Up")]

        [Label("Disable potion sickness audio")]
        [Tooltip("When enabled, plays a higher-pitched potion-drinking sound effect when potion sickness disappears.")]
        public bool DisablePSAudio;

        [Label("Disable potion sickness visual")]
        [Tooltip("When enabled, an icon will appear above the player when potion sickness disappears.")]
        public bool DisablePSVisual;

        [Header("[i:2701] Debuff Pop Up")]

        [Label("Disable buff pop ups")]
        public bool DisableBuffVisual;
    }
}
