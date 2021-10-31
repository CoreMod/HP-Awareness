using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace HPAware
{
    [Label("Configuration")]
    public class Modconfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Disable Moon Lord Shader")]
        [Tooltip("Moon Lord's shader may cause the overlays below to be invisible. Turn this on to disable the shader.")]
        public bool DisableMLShader;

        [Header("[i:29] Hurt Overlay")]

        [Label("Disable Overlay for Getting Hit")]
        [Tooltip("When damaged, this overlay makes the edges of the screen red before quickly disappearing. Turn this on to disable it.")]
        public bool DisableHurtOverlay;

        [Label("Overlay to use")]
        [OptionStrings(new string[] { "HPOverlay", "NewHPOverlay", "HPOverlayFlat" })]
        [DefaultValue("NewHPOverlay")]
        public string HurtOverlayType;

        [Label("Have variable intensity")]
        [Tooltip("Hurt overlay's opacity and size fluctuates depending on how often you get hurt.")]
        public bool HaveIntensity;

        [Header("[i:29] Low HP Overlay")]

        [Label("Disable overlay for low health")]
        [Tooltip("Whenever below a certain percentage of health, this overlay makes the edges of the screen red, fading in and out. Turn this on to disable it.")]
        public bool DisableLowHpOverlay;

        [Label("Use Classic Low HP Overlay")]
        [Tooltip("Instead of a gradient, it is a box.")]
        public bool ClassicLowHpOverlay;

        [Label("Low HP Percentage")]
        [Tooltip("Having HP lower or equal to this percentage activates the overlay.")]
        [DefaultValue(0.25)]
        public float Overlaytrigger;

        [Header("[i:705] Player HP Bar")]

        [Label("Disable player HP bar")]
        [Tooltip("When damaged, this appears below you much like every other HP bar. Turn this on to disable it.")]
        public bool DisableHPBar;

        [Header("[i:28] Potion Pop Up")]

        [Label("Disable Potion Sickness Audio")]
        [Tooltip("Plays a higher-pitched potion-drink sound effect when potion sickness disappears. Turn this on to disable it.")]
        public bool DisablePSAudio;

        [Label("Disable Potion Sickness Visual")]
        [Tooltip("A potion icon will appear above the player when potion sickness disappears. Turn this on to disable it.")]
        public bool DisablePSVisual;

        [Header("[i:2701] Debuff Pop Up")]

        [Label("Disable Debuff Pop Ups")]
        [Tooltip("When you get a debuff, the debuff's icon will appear above the player for a second. Turn this on to disable it.")]
        public bool DisableBuffVisual;
    }
}
