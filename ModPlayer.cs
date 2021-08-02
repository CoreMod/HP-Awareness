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
        private float Counter;
        private int PotionPopUp;
        private int DebuffTimer;
        public readonly List<int> Debuffs = new();

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI && !M.DisableHurtOverlay)
            {
                string Overlay = (!M.ClassicHurtOverlay) ? "NewHPOverlay" : "HPOverlay";
                Filters.Scene.Activate(Overlay);
                Counter = 30;
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
                        if (i == BuffID.Campfire || i == BuffID.HeartLamp || i == BuffID.PeaceCandle || i == BuffID.StarInBottle || i == BuffID.PotionSickness || i == BuffID.ManaSickness || i == BuffID.Sunflower || i == BuffID.MonsterBanner || i == BuffID.Werewolf || i == BuffID.Merfolk)
                        {
                            continue;
                        }
                        if (Main.LocalPlayer.HasBuff(i) && Main.debuff[i] && !Debuffs.Contains(i))
                        {
                            GetInstance<HPAwareSystem>().ShowDebuff();
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
                    GetInstance<HPAwareSystem>().HidePotion();
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI)
            {
                //Manipulate hurt shader
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
                //Show Low HP shader
                if (Player.statLife <= Player.statLifeMax2 * M.Overlaytrigger && !M.DisableLowHpOverlay)
                {
                    string Overlay = (!M.ClassicLowHpOverlay) ? "NewHPOverlay2" : "HPOverlay2";
                    Filters.Scene.Activate(Overlay);
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
                if (Filters.Scene["NewHPOverlay"].IsActive() && M.ClassicHurtOverlay)
                {
                    Filters.Scene["NewHPOverlay"].Deactivate();
                }
                if (Filters.Scene["HPOverlay"].IsActive() && !M.ClassicHurtOverlay)
                {
                    Filters.Scene["HPOverlay"].Deactivate();
                }
                if (Filters.Scene["NewHPOverlay2"].IsActive() && M.ClassicLowHpOverlay)
                {
                    Filters.Scene["NewHPOverlay2"].Deactivate();
                }
                if (Filters.Scene["HPOverlay2"].IsActive() && !M.ClassicLowHpOverlay)
                {
                    Filters.Scene["HPOverlay2"].Deactivate();
                }
            }
        }

        public override void UpdateDead()
        {
            Filters.Scene["HPOverlay2"].Deactivate();
            Filters.Scene["NewHPOverlay2"].Deactivate();
            if (Filters.Scene["HPOverlay"].IsActive() || Filters.Scene["NewHPOverlay"].IsActive())
            {
                Filters.Scene["HPOverlay"].Deactivate();
                Filters.Scene["NewHPOverlay"].Deactivate();
            }
            GetInstance<HPAwareSystem>().HideDebuff();
        }
    }
}