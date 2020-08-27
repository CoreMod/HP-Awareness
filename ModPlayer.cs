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
        Modconfig M = GetInstance<Modconfig>();
        private float Counter;
        private int PotionPopUp = 0;
        private int DebuffTimer;
        public readonly List<int> Debuffs = new List<int>();

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI && !M.DisableHurtOverlay)
            {
                if (!M.ClassicHurtOverlay)
                {
                    Filters.Scene.Activate("NewHPOverlay");
                }
                else
                {
                    Filters.Scene.Activate("HPOverlay");
                }
                Counter = 30;
            }
        }

        public override void PostUpdateBuffs()
        {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI)
            {
                if (!M.DisableBuffVisual)
                {
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

                    DebuffTimer--;

                    if (DebuffTimer <= 0)
                    {
                        GetInstance<HPAware>().HideDebuff();
                    }
                }

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

                PotionPopUp--;

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
                if (Filters.Scene["HPOverlay"].IsActive() || Filters.Scene["NewHPOverlay"].IsActive())
                {
                    Filters.Scene["HPOverlay"].GetShader().UseOpacity(Counter / 10);
                    Filters.Scene["NewHPOverlay"].GetShader().UseOpacity(Counter / 10);
                    Counter--;

                    if (Counter <= 0)
                    {
                        Filters.Scene["HPOverlay"].Deactivate();
                        Filters.Scene["NewHPOverlay"].Deactivate();
                    }
                }

                if (player.statLife <= player.statLifeMax2 * M.Overlaytrigger && !M.DisableLowHpOverlay)
                {
                    if (!M.ClassicLowHpOverlay)
                    {
                        Filters.Scene.Activate("NewHPOverlay2");
                    }
                    else
                    {
                        Filters.Scene.Activate("HPOverlay2");
                    }
                }
                else
                {
                    Filters.Scene["HPOverlay2"].Deactivate();
                    Filters.Scene["NewHPOverlay2"].Deactivate();
                }

                if (Filters.Scene["MoonLord"].IsActive() && M.DisableMLShader)
                {
                    Filters.Scene["MoonLord"].Deactivate();
                }
            }
        }

        public override void UpdateDead()
        {
            Filters.Scene["HPOverlay2"].Deactivate();
            
            if (Filters.Scene["HPOverlay"].IsActive())
            {
                Filters.Scene["HPOverlay"].Deactivate();
            }
            
            GetInstance<HPAware>().HideDebuff();
        }
    }
}