using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace HPAware.UI
{
    internal class PotionUI : UIState
    {
        readonly Modconfig M = GetInstance<Modconfig>();

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.dedServ && Main.myPlayer == Main.LocalPlayer.whoAmI)
            {
                Vector2 Position = new Vector2(Main.LocalPlayer.Center.X, Main.LocalPlayer.position.Y);
                Vector2 Offset = new Vector2(10, 65);
                if (Main.LocalPlayer.gravDir != 1f)
                {
                    Position.Y = (2 * Main.screenPosition.Y) + (float)Main.screenHeight - Position.Y;
                    Offset.Y = 110f;
                }
                Color Opacity = new Color(M.PotionOpacity, M.PotionOpacity, M.PotionOpacity, M.PotionOpacity);
                spriteBatch.Draw(GetTexture("HPAware/UI/PotionReady"), Position - Main.screenPosition - Offset, Opacity);
            } 
        }
    }
}
