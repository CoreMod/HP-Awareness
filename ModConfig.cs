using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using static Terraria.ModLoader.ModContent;
using Terraria.Graphics.Effects;

namespace HPAware
{
    public class Modconfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public bool DisableMLShader;

        [Header("Hurt")]     //--------------------------------

        [DefaultValue(true)]
        public bool EnableHurtOverlay;

        [OptionStrings(new string[] { "HPOverlay", "NewHPOverlay", "HPOverlayFlat" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("NewHPOverlay")]
        public string HurtOverlayType;

        [DefaultValue(typeof(Color), "255, 0, 0, 0")]
        public Color HurtColor;

        public bool HurtUseDarkColors;

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

        [DefaultValue(true)]
        public bool EnableLowHpOverlay;

        public bool ClassicLowHpOverlay;

        [DefaultValue(typeof(Color), "255, 0, 0, 0")]
        public Color LowHpColor;

        public bool LowHpUseDarkColors;

        [SliderColor(0, 0, 255)]
        [DefaultValue(0.25f)]
        public float Overlaytrigger;

        [Range(0.1f, 1f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float LowHpAlpha;

        [Range(0f, 15f)]
        [Increment(1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(4f)]
        public float LowHpFlash;

        public bool EnableLowHpAudio;

        [OptionStrings(new string[] { "Bell", "Heartbeat", "Mana Chirp", "Click", "Bell (No Pitch)" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("Bell")]
        public string LowHpSound;

        [Range(1, 180)]
        [Slider]
        [SliderColor(0, 0, 255)]
        [DefaultValue(35)]
        public int LowHpSdFreq;

        [Header("Gray")]   //--------------------------------

        public bool EnableGrayVision;

        [SliderColor(0, 0, 255)]
        [DefaultValue(0.25f)]
        public float GrayTrigger;

        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float GrayMaxIntensity;

        [Header("Bar")]   //--------------------------------

        [DefaultValue(true)]
        public bool EnableHPBar;

        [Range(10, 610)]
        [Slider]
        [Increment(10)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(90)]
        public int HPBarDelay;

        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float HPBarTrigger;

        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float HPBarOpacity;

        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float HPBarScale;

        [Header("Potion")]    //--------------------------------

        [DefaultValue(true)]
        public bool EnablePSAudio;

        [DefaultValue(true)]
        public bool EnablePSVisual;

        [Range(10, 180)]
        [Slider]
        [Increment(10)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(60)]
        public int PotionDelay;

        [SliderColor(0, 0, 255)]
        [DefaultValue(255)]
        public byte PotionOpacity;

        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float PotionScale;

        [Header("Debuff")]  //--------------------------------

        [DefaultValue(true)]
        public bool EnableBuffVisual;

        [OptionStrings(new string[] { "Most recent only", "Horizontal", "Vertical" })]
        [SliderColor(255, 0, 0)]
        [DrawTicks]
        [DefaultValue("Vertical")]
        public string BuffLayout;

        public bool EnableBuffTimer;

        [Range(10, 610)]
        [Slider]
        [Increment(10)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(60)]
        public int BuffDelay;

        [SliderColor(0, 0, 255)]
        [DefaultValue(255)]
        public byte BuffOpacity;

        [Range(0.3f, 2f)]
        [Increment(0.1f)]
        [SliderColor(0, 0, 255)]
        [DefaultValue(1f)]
        public float BuffScale;

        public bool EnableEasyAddToBL;

        public List<string> DebuffBL = new()
        { "Campfire", "PeaceCandle", "HeartLamp", "CatBast",
            "StarInBottle", "PotionSickness", "ManaSickness",
            "Sunflower", "MonsterBanner", "Werewolf", "Merfolk" };

        public override void OnChanged()
        {
            //Called whenever config updates mid-game
            if (!Main.gameMenu)
            {
                GetInstance<BuffSystems>().UpdateBlacklistedDebuffs();
                //This mod doesn't run on the server anyways but just in case (only on changed client)
                if (!Main.dedServ)
                {
                    HPAwareSystem H = GetInstance<HPAwareSystem>();
                    if (!EnableHPBar)
                    {
                        H.HideHPBar();
                    }
                    if (!EnablePSVisual)
                    {
                        H.HidePotion();
                    }
                    if (!EnableBuffVisual)
                    {
                        H.HideDebuff();
                    }
                    //Turn off alternate HP overlay if on
                    foreach (string Overlay in GetInstance<HPSystemPlayer>().HurtTypes)
                    {
                        if (!EnableHurtOverlay || (Overlay != HurtOverlayType && Filters.Scene[Overlay].IsActive()))
                        {
                            Filters.Scene[Overlay].Deactivate();
                        }
                    }
                    if (!EnableLowHpOverlay)
                    {
                        Filters.Scene["LowHPBasic"].Deactivate();
                        Filters.Scene["LowHPNew"].Deactivate();
                    }
                    else
                    {
                        if (!ClassicLowHpOverlay && Filters.Scene["LowHPBasic"].IsActive())
                        {
                            Filters.Scene["LowHPBasic"].Deactivate();
                        }
                        else if (ClassicLowHpOverlay && Filters.Scene["LowHPNew"].IsActive())
                        {
                            Filters.Scene["LowHPNew"].Deactivate();
                        }
                    }
                }
            }
        }

        [JsonExtensionData]     //Note: Must be from Newtonsoft.Json, System.Text.Json.Serialization doesn't work
        private IDictionary<string, JToken> OldData = new Dictionary<string, JToken>();

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            //This method updates old var names to their new ones if they were changed
            //Deserialization occurs on mod reload
            //Pre-1.0.5.0 var names
            bool Bool;
            if (OldData.TryGetValue("DisableHurtOverlay", out var Token))
            {
                Bool = Token.ToObject<bool>();
                EnableHurtOverlay = !Bool;
            }
            if (OldData.TryGetValue("DisableLowHpOverlay", out Token))
            {
                Bool = Token.ToObject<bool>();
                EnableLowHpOverlay = !Bool;
            }
            if (OldData.TryGetValue("DisableLowHpAudio", out Token))
            {
                Bool = Token.ToObject<bool>();
                EnableLowHpAudio = !Bool;
            }
            if (OldData.TryGetValue("DisableHPBar", out Token))
            {
                Bool = Token.ToObject<bool>();
                EnableHPBar = !Bool;
            }
            if (OldData.TryGetValue("DisablePSAudio", out Token))
            {
                Bool = Token.ToObject<bool>();
                EnablePSAudio = !Bool;
            }
            if (OldData.TryGetValue("DisablePSVisual", out Token))
            {
                Bool = Token.ToObject<bool>();
                EnablePSVisual = !Bool;
            }
            if (OldData.TryGetValue("DisableBuffVisual", out Token))
            {
                Bool = Token.ToObject<bool>();
                EnableBuffVisual = !Bool;
            }
            if (OldData.TryGetValue("DisableBuffTimer", out Token))
            {
                Bool = Token.ToObject<bool>();
                EnableBuffTimer = !Bool;
            }
            OldData.Clear();    //Required to prevent crashing
        }
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
