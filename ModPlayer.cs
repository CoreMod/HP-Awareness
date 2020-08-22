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
        private float Counter;
        private int PotionPopUp = 0;
        private int DebuffTimer;
        public readonly List<int> Debuffs = new List<int>();

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI && !GetInstance<Modconfig>().DisableHurtOverlay)
            {
                Filters.Scene.Activate("HPOverlay");
                Counter = 30;
            }
        }

        public override void PostUpdateBuffs()
        {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI)
            {
                if (!GetInstance<Modconfig>().DisableBuffVisual)
                {
                    for (int i = 0; i < BuffLoader.BuffCount; i++)
                    {
                        if (i == BuffID.Campfire || i == BuffID.HeartLamp || i == BuffID.PeaceCandle || i == BuffID.StarInBottle || i == BuffID.PotionSickness || i == BuffID.ManaSickness || i == BuffID.Sunflower || i == BuffID.MonsterBanner)
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
                    if (!GetInstance<Modconfig>().DisablePSAudio)
                    {
                        Main.PlaySound(SoundID.Item, -1, -1, 3, 1f, 0.3f);
                    }
                    if (!GetInstance<Modconfig>().DisablePSVisual)
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
                if (Filters.Scene["HPOverlay"].IsActive())
                {
                    Filters.Scene["HPOverlay"].GetShader().UseOpacity(Counter / 10);
                    Counter--;

                    if (Counter <= 0)
                    {
                        Filters.Scene["HPOverlay"].Deactivate();
                    }
                }

                if (player.statLife <= player.statLifeMax2 * GetInstance<Modconfig>().Overlaytrigger && !GetInstance<Modconfig>().DisableLowHpOverlay)
                {
                    Filters.Scene.Activate("HPOverlay2");
                }
                else
                {
                    Filters.Scene["HPOverlay2"].Deactivate();
                }

                if (Filters.Scene["MoonLord"].IsActive() && GetInstance<Modconfig>().DisableMLShader)
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