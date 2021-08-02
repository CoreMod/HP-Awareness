using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace HPAware.UI
{
    internal class DebuffUI : UIState
    {
        private UIImage DebuffIcon;

        public override void OnActivate()
        {
            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                if (i == BuffID.Campfire || i == BuffID.HeartLamp || i == BuffID.PeaceCandle || i == BuffID.StarInBottle || i == BuffID.PotionSickness || i == BuffID.ManaSickness || i == BuffID.Sunflower || i == BuffID.MonsterBanner || i == BuffID.Werewolf || i == BuffID.Merfolk)
                {
                    continue;
                }
                if (Main.LocalPlayer.HasBuff(i) && Main.debuff[i] && !GetInstance<Modplayer>().Debuffs.Contains(i))
                {
                    Texture2D texture = Terraria.GameContent.TextureAssets.Buff[i].Value;
                    DebuffIcon = new UIImage(texture);
                    Append(DebuffIcon);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 Position = Main.LocalPlayer.position - Main.screenPosition;
            MarginLeft = Position.X - 6;
            MarginTop = Position.Y - 40;
            Recalculate();
        }
    }
}
