using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace HPAware
{
    public enum LowHPSound
    {
        Bell,
        Heartbeat,
        ManaChirp,
        Click,
        BellNoPitch
    }

    public class HPSystemPlayer : ModPlayer
    {
        private readonly Modconfig M = GetInstance<Modconfig>();
        private readonly BuffSystems B = GetInstance<BuffSystems>();
        private readonly string[] HurtTypes = new string[] { "HPOverlay", "NewHPOverlay", "HPOverlayFlat" };

        private float ShaderFade;
        public int PotionTimer;
        public int DebuffTimer;
        private int BarTimer;
        public float BarAlpha;
        public int DebuffToShow;
        public List<int> DebuffsToShow = new(Player.MaxBuffs);

        public override void OnEnterWorld()
        {
            GetInstance<BuffSystems>().UpdateBlacklistedDebuffs();
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI)
            {
                string HurtOverlay = M.HurtOverlayType;
                if (!M.DisableHurtOverlay)
                {
                    Filters.Scene.Activate(HurtOverlay);
                    ShaderFade = 1f;
                }
                else if (Filters.Scene[HurtOverlay].IsActive())
                {
                    Filters.Scene[HurtOverlay].Deactivate();
                }
                
                if (!M.DisableHPBar)
                {
                    BarTimer = M.HPBarDelay;
                    BarAlpha = M.HPBarOpacity;
                    GetInstance<HPAwareSystem>().HideHPBar();
                    GetInstance<HPAwareSystem>().ShowHPBar();
                }
            }
        }

        public override void PostUpdateBuffs()
        {
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI)
            {
                if (!M.DisableBuffVisual)
                {
                    //Show debuff UI
                    for (int i = 0; i < Player.buffType.Length; i++)        //Checks if player has gotten a new debuff
                    {
                        int Type = Player.buffType[i];
                        int Time = Player.buffTime[i];
                        if (Main.debuff[Type] && Time > 0 && !DebuffsToShow.Contains(Type) && !B.DoNotShow[Type])
                        {
                            DebuffToShow = Type;
                            DebuffsToShow.Add(Type);
                            GetInstance<HPAwareSystem>().ShowDebuff();
                            DebuffTimer = M.BuffDelay;
                        }
                    }
                    for (int i = 0; i < DebuffsToShow.Count; i++)       //Removes debuffs player no longer has from lists or was recently added to BL (Self Note: Don't use foreach)
                    {
                        if (!Player.HasBuff(DebuffsToShow[i]) || B.DoNotShow[DebuffsToShow[i]])
                        {
                            DebuffsToShow.Remove(DebuffsToShow[i]);
                        }
                    }
                }
                //Hide debuff UI
                if (DebuffTimer > 0 && DebuffTimer <= 600)  //600+ is infinite
                {
                    DebuffTimer--;
                }
                if (DebuffTimer <= 0 || DebuffsToShow.Count == 0)
                {
                    GetInstance<HPAwareSystem>().HideDebuff();
                }

                //Show potion UI
                if (Player.potionDelay == 1)
                {
                    if (!M.DisablePSAudio)
                    {
                        SoundEngine.PlaySound(PotionRdySnd);
                    }
                    if (!M.DisablePSVisual)
                    {
                        GetInstance<HPAwareSystem>().ShowPotion();
                        PotionTimer = M.PotionDelay;
                    }
                }
                //Hide potion UI
                if (PotionTimer > 0)
                {
                    PotionTimer--;
                }
                if (PotionTimer <= 0)
                {
                    GetInstance<HPAwareSystem>().HidePotion();
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI)
            {
                string HurtOverlay = M.HurtOverlayType;
                //Manipulate hurt shader
                if (Filters.Scene[HurtOverlay].IsActive())
                {
                    float ShaderAlpha = MathHelper.Lerp(0f, M.HurtAlpha, ShaderFade);
                    Color Col = M.HurtColor;
                    if (false)
                    {
                        Col = new((M.HurtColor.G + M.HurtColor.B) / 2, (M.HurtColor.R + M.HurtColor.B) / 2, (M.HurtColor.R + M.HurtColor.G) / 2);
                    }
                    Filters.Scene[HurtOverlay].GetShader()
                        .UseOpacity(ShaderAlpha)
                        .UseColor(Col)
                        .UseIntensity((true) ? 1f : -1f);
                    //Animate shader based on config fade speed
                    if (ShaderFade > 0f)
                    {
                        ShaderFade -= (float)Math.Round(M.HurtSpeed * 0.01, 2);     //Computers can't calculate decimals perfectly, so this rounds it to its intended value
                        ShaderFade = (float)Math.Round(ShaderFade, 2);
                    }
                    if (ShaderAlpha <= 0f && M.HaveIntensity)
                    {
                        Filters.Scene[HurtOverlay].Deactivate();
                    }
                }

                //Low HP effects
                if (Player.statLife <= Player.statLifeMax2 * M.Overlaytrigger)
                {
                    //Show Low HP shader
                    if (!M.DisableLowHpOverlay)
                    {
                        string LowOverlay = (!M.ClassicLowHpOverlay) ? "LowHPNew" : "LowHPBasic";
                        Filters.Scene.Activate(LowOverlay);
                        Filters.Scene[LowOverlay].GetShader()
                            .UseOpacity(M.LowHpAlpha)
                            .UseIntensity(M.LowHpFlash)
                            .UseColor(M.LowHpColor);
                    }
                    //SFX
                    if (Main.GameUpdateCount % M.LowHpSdFreq == 0 && !M.DisableLowHpAudio)
                    {
                        SoundStyle SoundToUse = Bell;       //Default
                        switch (M.LowHpSound)
                        {
                            case "Heartbeat":
                                SoundToUse = Heartbeat;
                                break;
                            case "Mana Chirp":
                                SoundToUse = ManaChirp;
                                break;
                            case "Click":
                                SoundToUse = Click;
                                break;
                            case "Bell (No Pitch)":
                                SoundToUse = BellNoPitch;
                                break;
                        }
                        SoundEngine.PlaySound(SoundToUse);
                    }
                }
                else
                {
                    Filters.Scene["LowHPBasic"].Deactivate();
                    Filters.Scene["LowHPNew"].Deactivate();
                }

                //Gray Vision
                Filter Gray = Filters.Scene["FlatGrayScale"];
                if (!M.DisableGrayVision && Player.statLife <= Player.statLifeMax2 * M.GrayTrigger)
                {
                    float HPPercent = (float)Player.statLife / ((float)Player.statLifeMax2 * M.GrayTrigger);
                    if (!Gray.IsActive())
                    {
                        Filters.Scene.Activate("FlatGrayScale");
                    }
                    else
                    {
                        float Intensity = Math.Clamp((1f - HPPercent) * M.GrayMaxIntensity, 0f, M.GrayMaxIntensity);
                        Gray.GetShader().UseIntensity(Intensity);  //0f = Full gray
                    }
                }
                else
                {
                    if (Gray.IsActive())   //Doesn't deactivate immediately, set to no gray until it happens
                    {
                        Gray.GetShader().UseIntensity(0f);
                    }
                    Gray.Deactivate();
                }

                //Manage HP bar
                if (BarTimer > 0 && BarTimer <= 600)  //600+ is infinite
                {
                    BarTimer--;
                }
                if (BarAlpha > 0f && BarTimer <= 0)
                {
                    BarAlpha -= 0.1f;
                }
                if (BarAlpha <= 0f)
                {
                    GetInstance<HPAwareSystem>().HideHPBar();
                }

                //Hide Moon Lord shader
                if (Filters.Scene["MoonLord"].IsActive() && M.DisableMLShader)
                {
                    Filters.Scene["MoonLord"].Deactivate();
                }
                //Turn off alternate HP overlay if on
                foreach (string Overlay in HurtTypes)
                {
                    if (Overlay != HurtOverlay && Filters.Scene[Overlay].IsActive())
                    {
                        Filters.Scene[Overlay].Deactivate();
                    }
                }
                if (!M.ClassicLowHpOverlay && Filters.Scene["LowHPBasic"].IsActive())
                {
                    Filters.Scene["LowHPBasic"].Deactivate();
                }
                else if (M.ClassicLowHpOverlay && Filters.Scene["LowHPNew"].IsActive())
                {
                    Filters.Scene["LowHPNew"].Deactivate();
                }
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (!Main.dedServ && !Main.gameMenu && Main.myPlayer == Player.whoAmI && !Lighting.NotRetro && drawInfo.shadow == 0f && !Player.DeadOrGhost)
            {
                if (ShaderFade > 0 && !M.DisableHurtOverlay)     //Screen shaders do not appear on retro/trippy lighting modes, this attempts to compensate
                {
                    string HurtOverlay = M.HurtOverlayType;
                    float ShaderAlpha = MathHelper.Lerp(0f, M.HurtAlpha, ShaderFade);
                    Color Col = new(ShaderAlpha, 0f, 0f, 0f);
                    switch (HurtOverlay)
                    {
                        case "HPOverlayFlat":
                            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new(0, 0, Main.screenWidth, Main.screenHeight), Col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                            break;
                        default:
                            DrawBorders(0.08, 0.03, Col);
                            break;
                    }
                }
                if (Player.statLife <= Player.statLifeMax2 * M.Overlaytrigger && !M.DisableLowHpOverlay)
                {
                    Color Col = new((float)((Math.Sin(M.LowHpFlash * Main.GlobalTimeWrappedHourly) + 1) * M.LowHpAlpha), 0f, 0f, 0f);
                    DrawBorders(0.03, 0.015, Col);
                }
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            //Disable every UI and overlay on death
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI)
            {
                ShaderFade = 0f;
                Filters.Scene["LowHPBasic"].Deactivate();
                Filters.Scene["LowHPNew"].Deactivate();
                Filters.Scene["FlatGrayScale"].Deactivate();
                foreach (string Overlay in HurtTypes)
                {
                    if (Filters.Scene[Overlay].IsActive())
                    {
                        Filters.Scene[Overlay].Deactivate();
                    }
                }
                GetInstance<HPAwareSystem>().HideDebuff();
                GetInstance<HPAwareSystem>().HidePotion();
                GetInstance<HPAwareSystem>().HideHPBar();
            }
        }

        private static void DrawBorders(double TopHeight, double SideWidth, Color Col)
        {
            Rectangle TopRect = new(0, 0, Main.screenWidth, (int)(Main.screenHeight * TopHeight));
            Rectangle SideRect = new(0, 0, (int)(Main.screenWidth * SideWidth), Main.screenHeight - (TopRect.Height * 2));
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero + Main.GameViewMatrix.Translation, TopRect, Col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);                                                 //Top row
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, SideRect.BottomLeft() + new Vector2(0f, TopRect.Height) - Main.GameViewMatrix.Translation, TopRect, Col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);      //Bottom row
            SideRect.Height = (Main.screenHeight - TopRect.Height * 2) - (int)(Main.GameViewMatrix.Translation.Y * 2);
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, TopRect.BottomLeft() + Main.GameViewMatrix.Translation, SideRect, Col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);      //Left column
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, TopRect.BottomRight() - new Vector2(SideRect.Width, 0f) + new Vector2(-Main.GameViewMatrix.Translation.X, Main.GameViewMatrix.Translation.Y), SideRect, Col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);     //Right column
        }

        public static readonly SoundStyle PotionRdySnd = new("Terraria/Sounds/Item_3")
        {
            Volume = 1f,
            Pitch = 0.3f
        };

        //Low HP sound effects
        public static readonly SoundStyle Bell = new("Terraria/Sounds/Item_35")
        {
            Volume = 1f,
            Pitch = 0.8f
        };

        public static readonly SoundStyle Heartbeat = new("Terraria/Sounds/Custom/deerclops_step")
        {
            Volume = 1f,
            Pitch = -1f
        };

        public static readonly SoundStyle ManaChirp = new("Terraria/Sounds/MaxMana")
        {
            Volume = 1f,
            Pitch = -0.6f
        };

        public static readonly SoundStyle Click = new("Terraria/Sounds/Drip_1")
        {
            Volume = 1f,
            Pitch = 0.5f
        };

        public static readonly SoundStyle BellNoPitch = new("Terraria/Sounds/Item_35")      //Mostly here to be replaced by a resource pack
        {
            Volume = 1f,
            PitchRange = (0f, 0f),
            MaxInstances = 1
        };
    }

    public class BuffSystems : GlobalBuff
    {
        private Modconfig M = GetInstance<Modconfig>();

        /// <summary>
        /// Contains all currently loaded vanilla and modded buffs. If true, buff will not be shown in pop up. 
        /// </summary>
        public bool[] DoNotShow = new bool[BuffLoader.BuffCount];

        private static LocalizedText AddToBLText;

        public override void SetStaticDefaults()
        {
            //Get text from localization files
            AddToBLText = Mod.GetLocalization(nameof(AddToBLText));
        }

        public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
        {
            if (Main.debuff[type] && !DoNotShow[type] && M.EnableEasyAddToBL)
            {
                //Add tooltip hint for adding to BL
                tip += $"\n[i:2701] {AddToBLText}";
            }
        }

        public override bool RightClick(int type, int buffIndex)
        {
            if (Main.debuff[type] && !DoNotShow[type] && M.EnableEasyAddToBL)
            {
                //Add to BL and save change
                SoundEngine.PlaySound(SoundID.Item20);
                M.DebuffBL.Add(BuffID.Search.GetName(type));
                M.SaveChanges();    //Since this is a client config with no reload changes this is fine
            }
            return base.RightClick(type, buffIndex);
        }

        /// <summary>
        /// Takes config debuffs and flags them, preventing them from being shown.
        /// </summary>
        public void UpdateBlacklistedDebuffs()
        {
            DoNotShow = new bool[BuffLoader.BuffCount];
            foreach (string BLDebuff in M.DebuffBL)
            {
                if (BuffID.Search.TryGetId(BLDebuff, out int ID))
                {
                    DoNotShow[ID] = true;
                }
            }
        }
    }
}