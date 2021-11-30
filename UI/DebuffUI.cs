using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace HPAware.UI
{
    internal class DebuffUI : UIState
    {
        readonly Modconfig M = GetInstance<Modconfig>();

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.dedServ && Main.myPlayer == Main.LocalPlayer.whoAmI)
            {
                Vector2 Position = new Vector2(Main.LocalPlayer.Center.X, Main.LocalPlayer.position.Y);
                Vector2 Offset = new Vector2(16, 40);
                if (Main.LocalPlayer.gravDir != 1f)
                {
                    Position.Y = (2 * Main.screenPosition.Y) + (float)Main.screenHeight - Position.Y;
                    Offset.Y = 85f;
                }
                Color Opacity = new Color(M.BuffOpacity, M.BuffOpacity, M.BuffOpacity, M.BuffOpacity);
                spriteBatch.Draw(Main.buffTexture[Main.LocalPlayer.GetModPlayer<Modplayer>().DebuffToShow], Position - Main.screenPosition - Offset, Opacity);
            }
        }
    }
}
