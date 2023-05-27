using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace HPAware
{
    public class Modconfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public bool DisableMLShader;

        [Header("Hurt")]     //--------------------------------

        public bool DisableHurtOverlay;

        [OptionStrings(new string[] { "HPOverlay", "NewHPOverlay", "HPOverlayFlat" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("NewHPOverlay")]
        public string HurtOverlayType;

        [Range(0.1f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1.5f)]
        public float HurtAlpha;

        [Range(1, 10)]
        [Slider]
        [SliderColor(0, 0, 255)]
        [DefaultValue(6)]
        public int HurtSpeed;

        public bool HaveIntensity;

        [Header("Low")]   //--------------------------------

        public bool DisableLowHpOverlay;

        public bool ClassicLowHpOverlay;

        [Range(0.1f, 1f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float LowHpAlpha;

        [Range(1f, 15f)]
        [Increment(1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(4f)]
        public float LowHpFlash;

        [DefaultValue(true)]
        public bool DisableLowHpAudio;

        [OptionStrings(new string[] { "Bell", "Heartbeat", "Mana Chirp", "Click" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("Bell")]
        public string LowHpSound;

        [Range(1, 180)]
        [Slider]
        [SliderColor(0, 0, 255)]
        [DefaultValue(35)]
        public int LowHpSdFreq;

        [SliderColor(0, 0, 255)]
        [DefaultValue(0.25)]
        public float Overlaytrigger;

        [Header("Bar")]   //--------------------------------

        public bool DisableHPBar;

        [Range(0, 600)]
        [Slider]
        [Increment(10)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(90)]
        public int HPBarDelay;

        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float HPBarOpacity;

        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float HPBarScale;

        [Header("Potion")]    //--------------------------------

        public bool DisablePSAudio;

        public bool DisablePSVisual;

        [SliderColor(0, 0, 255)]
        [DefaultValue(255)]
        public byte PotionOpacity;

        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float PotionScale;

        [Header("Debuff")]  //--------------------------------

        public bool DisableBuffVisual;

        [OptionStrings(new string[] { "Most recent only", "Horizontal", "Vertical" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("Vertical")]
        public string BuffLayout;

        [SliderColor(0, 0, 255)]
        [DefaultValue(255)]
        public byte BuffOpacity;

        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float BuffScale;

        public List<string> DebuffBL = new()
        { "Campfire", "PeaceCandle", "HeartLamp", "CatBast",
            "StarInBottle", "PotionSickness", "ManaSickness",
            "Sunflower", "MonsterBanner", "Werewolf", "Merfolk" };
    }

    public class NBuffList : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("List")]
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
