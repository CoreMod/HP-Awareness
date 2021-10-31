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
        private int PotionPopUp;
        private int DebuffTimer;
        public readonly List<int> Debuffs = new List<int>();

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI && !M.DisableHurtOverlay)
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
                if (!M.DisableHPBar)    //Show HP bar, UI handles the rest
                {
                    GetInstance<HPAware>().HideHPBar();
                    GetInstance<HPAware>().ShowHPBar();
                }
            }
        }

        public override void PostUpdateBuffs()
        {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI)
            {
                if (!M.DisableBuffVisual)
                {
                    //Show debuff UI
                    for (int i = 0; i < BuffLoader.BuffCount; i++)
                    {
                        if (i == BuffID.Campfire || i == BuffID.HeartLamp || i == BuffID.PeaceCandle || i == BuffID.StarInBottle || i == BuffID.PotionSickness || i == BuffID.ManaSickness || i == BuffID.Sunflower || i == BuffID.MonsterBanner || i == BuffID.Werewolf || i == BuffID.Merfolk)
                        {
                            continue;
                        }
                        if (Main.LocalPlayer.HasBuff(i) && Main.debuff[i] && !Debuffs.Contains(i))
                        {
                            GetInstance<HPAware>().ShowDebuff();
                            DebuffTimer = 60;
                            Debuffs.Add(i);
                        }
                        else if (!Main.LocalPlayer.HasBuff(i) && Debuffs.Contains(i))
                        {
                            Debuffs.Remove(i);
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
                    GetInstance<HPAware>().HideDebuff();
                }
                //Show potion UI
                if (player.potionDelay == 1)
                {
                    if (!M.DisablePSAudio)
                    {
                        Main.PlaySound(SoundID.Item, -1, -1, 3, 1f, 0.3f);
                    }
                    if (!M.DisablePSVisual)
                    {
                        GetInstance<HPAware>().ShowPotion();
                        PotionPopUp = 60;
                    }
                }
                //Hide potion UI
                if (PotionPopUp > 0)
                {
                    PotionPopUp--;
                }
                if (PotionPopUp <= 0)
                {
                    GetInstance<HPAware>().HidePotion();
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI)
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
                if (player.statLife <= player.statLifeMax2 * M.Overlaytrigger && !M.DisableLowHpOverlay)
                {
                    string LowOverlay = (!M.ClassicLowHpOverlay) ? "NewHPOverlay2" : "HPOverlay2";
                    Filters.Scene.Activate(LowOverlay);
                }
                else
                {
                    Filters.Scene["HPOverlay2"].Deactivate();
                    Filters.Scene["NewHPOverlay2"].Deactivate();
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
            if (!Main.dedServ && Main.myPlayer == player.whoAmI)
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
                GetInstance<HPAware>().HideDebuff();
                GetInstance<HPAware>().HidePotion();
                GetInstance<HPAware>().HideHPBar();
            }
        }
    }
}