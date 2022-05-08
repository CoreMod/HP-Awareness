using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace HPAware
{
    public class Modplayer : ModPlayer
    {
        readonly Modconfig M = GetInstance<Modconfig>();
        readonly string[] HurtTypes = new string[] { "HPOverlay", "NewHPOverlay", "HPOverlayFlat" };
        private float ShaderAlpha;
        public int PotionTimer;
        public int DebuffTimer;
        private int BarTimer;
        public float BarAlpha;
        private readonly List<int> Debuffs = new();
        public int DebuffToShow;
        public List<int> DebuffsToShow = new();

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI)
            {
                string HurtOverlay = GetInstance<Modconfig>().HurtOverlayType;
                if (!M.DisableHurtOverlay)
                {
                    Filters.Scene.Activate(HurtOverlay);
                    ShaderAlpha = 2f;
                }
                else if (Filters.Scene[HurtOverlay].IsActive())
                {
                    Filters.Scene[HurtOverlay].Deactivate();
                }
                if (!M.DisableHPBar)    //UI handles the rest
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
                    for (int i = 0; i < BuffLoader.BuffCount; i++)
                    {
                        foreach (string BLDebuff in M.DebuffBL)
                        {
                            if (i != 0 && BuffID.Search.GetName(i) == BLDebuff && !Debuffs.Contains(i))      //Check prevents modded buffs from being added into list if said mod is unloaded
                            {
                                Debuffs.Add(i);     //Prevents debuff from being shown due to it already being in the list when checked later (List takes IDs instead of keys)
                            }
                        }
                        if (Main.LocalPlayer.HasBuff(i) && Main.debuff[i] && !Debuffs.Contains(i))
                        {
                            DebuffToShow = i;
                            DebuffsToShow.Add(i);
                            GetInstance<HPAwareSystem>().ShowDebuff();
                            DebuffTimer = 60;
                            Debuffs.Add(i);
                        }
                        else if (!Main.LocalPlayer.HasBuff(i) && Debuffs.Contains(i))
                        {
                            Debuffs.Remove(i);
                            DebuffsToShow.Remove(i);
                        }
                    }
                }
                //Hide debuff UI
                if (DebuffTimer > 0)
                {
                    DebuffTimer--;
                }
                if (DebuffTimer <= 0)
                {
                    GetInstance<HPAwareSystem>().HideDebuff();
                }
                //Show potion UI
                if (Player.potionDelay == 1)
                {
                    if (!M.DisablePSAudio)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, -1, -1, 3, 1f, 0.3f);
                    }
                    if (!M.DisablePSVisual)
                    {
                        GetInstance<HPAwareSystem>().ShowPotion();
                        PotionTimer = 60;
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
                string HurtOverlay = GetInstance<Modconfig>().HurtOverlayType;
                //Manipulate hurt shader
                if (Filters.Scene[HurtOverlay].IsActive())
                {
                    Filters.Scene[HurtOverlay].GetShader().UseOpacity(ShaderAlpha);
                    if (ShaderAlpha > 0)
                    {
                        ShaderAlpha -= 0.1f;
                    }
                    if (ShaderAlpha <= 0 && M.HaveIntensity)
                    {
                        Filters.Scene[HurtOverlay].Deactivate();
                    }
                }
                //Show Low HP shader
                if (Player.statLife <= Player.statLifeMax2 * M.Overlaytrigger && !M.DisableLowHpOverlay)
                {
                    string LowOverlay = (!M.ClassicLowHpOverlay) ? "NewHPOverlay2" : "HPOverlay2";
                    Filters.Scene.Activate(LowOverlay);
                }
                else
                {
                    Filters.Scene["HPOverlay2"].Deactivate();
                    Filters.Scene["NewHPOverlay2"].Deactivate();
                }
                if (Player.statLife <= Player.statLifeMax2 * M.Overlaytrigger && Main.GameUpdateCount % 35 == 0 && !M.DisableLowHpAudio)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, -1, -1, 35, 1f, 0.8f);
                }
                //Manage HP bar
                if (BarTimer > 0)
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
                if (!M.ClassicLowHpOverlay && Filters.Scene["HPOverlay2"].IsActive())
                {
                    Filters.Scene["HPOverlay2"].Deactivate();
                }
                else if (M.ClassicLowHpOverlay && Filters.Scene["NewHPOverlay2"].IsActive())
                {
                    Filters.Scene["NewHPOverlay2"].Deactivate();
                }
            }
        }

        public override void UpdateDead()
        {
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI)
            {
                Filters.Scene["HPOverlay2"].Deactivate();
                Filters.Scene["NewHPOverlay2"].Deactivate();
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
    }
}