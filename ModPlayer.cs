using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace HPAware
{
    public class Modplayer : ModPlayer
    {
        private bool Overlay;
        private float Counter;
        private bool Potion;
        private int PotionPopUp = 0;

        public override void ResetEffects()
        {
            Overlay = false;
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (!GetInstance<Modconfig>().DisableHurtOverlay)
            {
                Overlay = true;
                Counter = 30;
            }
        }

        public override void UpdateBiomeVisuals()
        {
            if (!Main.dedServ && Overlay)
            {
                Filters.Scene.Activate("HPOverlay");
            }
        }

        public override void PostUpdate()
        {
            if (Filters.Scene["HPOverlay"].IsActive())
            {
                Filters.Scene["HPOverlay"].GetShader().UseOpacity(Counter / 10);
                Counter--;

                if (Counter == 0)
                {
                    Filters.Scene["HPOverlay"].Deactivate();
                }
            }

            if (!Main.dedServ && player.statLife <= player.statLifeMax2 * GetInstance<Modconfig>().Overlaytrigger && !GetInstance<Modconfig>().DisableLowHpOverlay)
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

            if (!Main.dedServ && player.potionDelay == 1)
            {
                if (!GetInstance<Modconfig>().DisablePSAudio)
                {
                    Main.PlaySound(SoundID.Item, -1, -1, 3, 1f, 0.3f);
                }                
                if (!GetInstance<Modconfig>().DisablePSVisual)
                {
                    Potion = true;
                }                             
            }
        }

        public static readonly PlayerLayer PotionReady = new PlayerLayer("HPAware", "PotionReady", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("HPAware");
            Modplayer modPlayer = drawPlayer.GetModPlayer<Modplayer>();

            if (modPlayer.Potion)
            {
                if (modPlayer.PotionPopUp <= 40)
                {
                    modPlayer.PotionPopUp++;
                }
                else
                {
                    modPlayer.Potion = false;
                    modPlayer.PotionPopUp = 0;
                }
                Texture2D texture = mod.GetTexture("UI/PotionReady");
                float drawX = (int)(drawInfo.position.X + drawPlayer.width / 2 - Main.screenPosition.X + 4);
                float drawY = (int)(drawInfo.position.Y + drawPlayer.height / 2 - Main.screenPosition.Y - 50);
                DrawData drawData = new DrawData(texture, new Vector2(drawX, drawY)/*position*/, null/*sourcerect*/, Color.White, 0f/*rotation*/, new Vector2(texture.Width / 2f, texture.Height / 2f)/*center of sprite*/, 1f/*scale*/, SpriteEffects.None, 0/*inactivelayerdepth*/);
                Main.playerDrawData.Add(drawData);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            layers.Add(PotionReady);
        }

        public override void UpdateDead()
        {
            Filters.Scene["HPOverlay2"].Deactivate();
        }
    }
}
