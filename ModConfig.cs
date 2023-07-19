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
        [Tooltip("Moon Lord's shader may cause the 2 overlays below to be invisible\nCheck the box to disable")]
        public bool DisableMLShader;

        [Header("[i:29] Hurt Overlay")]     //--------------------------------

        [Label("Disable Overlay for Getting Hit")]
        [Tooltip("When damaged, overlay makes the edges of the screen red before quickly disappearing\nCheck the box to disable")]
        public bool DisableHurtOverlay;

        [Label("Overlay to use")]
        [Tooltip("Determines overlay type \nHPOverlay - Single color, covers screen edges (Retro/trippy lighting default)\nNew - Gradient, covers screen edges (Color/white lighting mode only)\nFlat - Single color, fills screen")]
        [OptionStrings(new string[] { "HPOverlay", "NewHPOverlay", "HPOverlayFlat" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("NewHPOverlay")]
        public string HurtOverlayType;

        [Label("Overlay intensity")]
        [Tooltip("Determines overlay color intensity\nHigher = More intense")]
        [Range(0.1f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1.5f)]
        public float HurtAlpha;

        [Label("Overlay fade speed")]
        [Tooltip("Determines how fast the overlay disappears\nHigher = Quicker")]
        [Range(1, 10)]
        [Slider]
        [SliderColor(0, 0, 255)]
        [DefaultValue(6)]
        public int HurtSpeed;

        [Label("Use variable intensity")]
        [Tooltip("Hurt overlay's intensity fluctuates depending on how often you get hurt\nRecommended with higher intensity values\nColor/white lighting mode only")]
        public bool HaveIntensity;

        [Header("[i:29] Low HP Overlay")]   //--------------------------------

        [Label("Disable overlay for low health")]
        [Tooltip("When active, overlay makes the edges of the screen pulsate red\nCheck the box to disable")]
        public bool DisableLowHpOverlay;

        [Label("Use Classic Low HP Overlay")]
        [Tooltip("Instead of a gradient, it is a single color\nRetro/trippy lighting default")]
        public bool ClassicLowHpOverlay;

        [Label("Overlay intensity")]
        [Tooltip("Determines overlay color intensity\nHigher = More intense")]
        [Range(0.1f, 1f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float LowHpAlpha;

        [Label("Overlay flashing rate")]
        [Tooltip("Determines rate of flashing\nHigher = More flashing")]
        [Range(1f, 15f)]
        [Increment(1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(4f)]
        public float LowHpFlash;

        [Label("Disable Audio")]
        [Tooltip("When active, a sound plays repeatedly\nCheck the box to disable")]
        [DefaultValue(true)]
        public bool DisableLowHpAudio;

        [Label("Sound to use")]
        [Tooltip("Determines sound")]
        [OptionStrings(new string[] { "Bell", "Heartbeat", "Mana Chirp", "Click" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("Bell")]
        public string LowHpSound;

        [Label("Sound frequency")]
        [Tooltip("Determines how often the sound plays (in ticks)\nLower = More often\n60 ticks = 1 second")]
        [Range(1, 180)]
        [Slider]
        [SliderColor(0, 0, 255)]
        [DefaultValue(35)]
        public int LowHpSdFreq;

        [Label("Low HP Percentage")]
        [Tooltip("Having HP lower or equal to this percentage activates overlay & sound")]
        [SliderColor(0, 0, 255)]
        [DefaultValue(0.25)]
        public float Overlaytrigger;

        [Header("[i:705] Player HP Bar")]   //--------------------------------

        [Label("Disable player HP bar")]
        [Tooltip("When damaged, this appears below you much like every other HP bar\nCheck the box to disable")]
        public bool DisableHPBar;

        [Label("Bar Duration")]
        [Tooltip("Determines how long it lingers on screen (in ticks)\n60 ticks = 1 second")]
        [Range(10, 600)]
        [Slider]
        [Increment(10)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(90)]
        public int HPBarDelay;

        [Label("Bar Opacity")]
        [Tooltip("Determines bar transparency")]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float HPBarOpacity;

        [Label("Icon Scale")]
        [Tooltip("Determines bar size")]
        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float HPBarScale;

        [Header("[i:28] Potion Pop Up")]    //--------------------------------

        [Label("Disable Potion Sickness Audio")]
        [Tooltip("Plays a higher-pitched potion-drink sound effect when potion sickness disappears\nCheck the box to disable")]
        public bool DisablePSAudio;

        [Label("Disable Potion Sickness Visual")]
        [Tooltip("A potion icon will appear above the player when potion sickness disappears\nCheck the box to disable")]
        public bool DisablePSVisual;

        [Label("Visual Duration")]
        [Tooltip("Determines how long icon lingers on screen (in ticks)\n60 ticks = 1 second")]
        [Range(10, 180)]
        [Slider]
        [Increment(10)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(60)]
        public int PotionDelay;

        [Label("Icon Opacity")]
        [Tooltip("Determines transparency of the icon\n255 = Fully visible\n0 = Invisible")]
        [SliderColor(0, 0, 255)]
        [DefaultValue(255)]
        public byte PotionOpacity;

        [Label("Icon Scale")]
        [Tooltip("Determines icon size")]
        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float PotionScale;

        [Header("[i:2701] Debuff Pop Up\n(NOTE: Fargo's Mutant Mod adds a similar feature. If these don't seem to work, check their config)")]  //--------------------------------

        [Label("Disable Debuff Pop Ups")]
        [Tooltip("When you get a debuff, the debuff's icon(s) will appear above the player for a second\nCheck the box to disable")]
        public bool DisableBuffVisual;

        [Label("Layout to use")]
        [Tooltip("Determines icon layout\nMost recent only - shows the most recently gained debuff\nOther two show every current debuff in an orientation")]
        [OptionStrings(new string[] { "Most recent only", "Horizontal", "Vertical" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("Vertical")]
        public string BuffLayout;

        [Label("Disable Showing Debuff Duration")]
        [Tooltip("Debuff icons will have their duration placed on top of them\nCheck the box to disable")]
        [DefaultValue(true)]
        public bool DisableBuffTimer;

        [Label("Icon Duration")]
        [Tooltip("Determines how long icon(s) linger on screen (in ticks)\n60 ticks = 1 second")]
        [Range(10, 300)]
        [Slider]
        [Increment(10)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(60)]
        public int BuffDelay;

        [Label("Icon Opacity")]
        [Tooltip("Determines transparency of the icon(s)\n255 = Fully visible\n0 = Invisible")]
        [SliderColor(0, 0, 255)]
        [DefaultValue(255)]
        public byte BuffOpacity;

        [Label("Icon Scale")]
        [Tooltip("Determines icon size(s)")]
        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float BuffScale;

        [Label("Debuff Blacklist")]
        [Tooltip("Any debuffs in this list will NOT have a pop up (See next page for options)\nReload mods for best effects")]
        public List<string> DebuffBL = new()
        { "Campfire", "PeaceCandle", "HeartLamp", "CatBast",
            "StarInBottle", "PotionSickness", "ManaSickness",
            "Sunflower", "MonsterBanner", "Werewolf", "Merfolk" };
    }

    [Label("Debuff List")]
    public class NBuffList : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Copy & paste desired debuffs below over to blacklist." +
            "\nMust be in-world to view, use restore defaults to refresh list." +
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
