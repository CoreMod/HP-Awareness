using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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

        [Header("[i:29] Hurt Overlay")]     //--------------------------------

        [Label("Disable Overlay for Getting Hit")]
        [Tooltip("When damaged, this overlay makes the edges of the screen red before quickly disappearing. Turn this on to disable it.")]
        public bool DisableHurtOverlay;

        [Label("Overlay to use")]
        [OptionStrings(new string[] { "HPOverlay", "NewHPOverlay", "HPOverlayFlat" })]
        [DrawTicks]
        [DefaultValue("NewHPOverlay")]
        public string HurtOverlayType;

        [Label("Have variable intensity")]
        [Tooltip("Hurt overlay's opacity and size fluctuates depending on how often you get hurt.")]
        public bool HaveIntensity;

        [Header("[i:29] Low HP Overlay")]   //--------------------------------

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

        [Header("[i:705] Player HP Bar")]   //--------------------------------

        [Label("Disable player HP bar")]
        [Tooltip("When damaged, this appears below you much like every other HP bar. Turn this on to disable it.")]
        public bool DisableHPBar;

        [Label("Bar Delay")]
        [Tooltip("Determines how long in ticks it lingers before disappearing")]
        [Range(0, 600)]
        [Slider]
        [Increment(10)]
        [DefaultValue(90)]
        public int HPBarDelay;

        [Label("Bar Opacity")]
        [Tooltip("Determines transparency of the bar")]
        [DefaultValue(1f)]
        public float HPBarOpacity;

        [Header("[i:28] Potion Pop Up")]    //--------------------------------

        [Label("Disable Potion Sickness Audio")]
        [Tooltip("Plays a higher-pitched potion-drink sound effect when potion sickness disappears. Turn this on to disable it.")]
        public bool DisablePSAudio;

        [Label("Disable Potion Sickness Visual")]
        [Tooltip("A potion icon will appear above the player when potion sickness disappears. Turn this on to disable it.")]
        public bool DisablePSVisual;

        [Label("Icon Opacity")]
        [Tooltip("Determines transparency of the icon (255 = Fully visible, 0 = Invisible)")]
        [DefaultValue(255)]
        public byte PotionOpacity;

        [Header("[i:2701] Debuff Pop Up")]  //--------------------------------

        [Label("Disable Debuff Pop Ups")]
        [Tooltip("When you get a debuff, the debuff's icon will appear above the player for a second. Turn this on to disable it.")]
        public bool DisableBuffVisual;

        [Label("Icon Opacity")]
        [Tooltip("Determines transparency of the icon (255 = Fully visible, 0 = Invisible)")]
        [DefaultValue(255)]
        public byte BuffOpacity;

        [Label("Debuff Blacklist")]
        [Tooltip("Any debuffs in this list will NOT have a pop up (See next page for options)")]
        public List<string> DebuffBL = new()
        { "Campfire", "PeaceCandle", "HeartLamp", "CatBast",
            "StarInBottle", "PotionSickness", "ManaSickness",
            "Sunflower", "MonsterBanner", "Werewolf", "Merfolk" };
    }

    [Label("Debuff List")]
    public class NBuffList : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Must be in-world to view, use reset defaults to refresh list." +
            "\nInteract w/ textbox to use copy & paste." +
            "\nCheck \"HPAware_NBuffList\" file in your Mod Configs folder for better list view (make & save any changes for it to appear/update first).")]
        [Label("Debuffs")]
        public DebuffList DebuffList = new();
    }

    public class DebuffList
    {
        public List<string> Debuffs = new();

        public DebuffList()
        {
            if (!Main.gameMenu)     //Only works in-world to prevent a mod loading after this one from ruining everything
            {
                for (int i = 0; i < BuffLoader.BuffCount; i++)
                {
                    if (Main.debuff[i])
                    {
                        Debuffs.Add(BuffID.Search.GetName(i));
                    }
                }
            }
        }
    }
}
