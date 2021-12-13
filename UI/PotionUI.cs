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
                Vector2 Position = new(Main.LocalPlayer.Center.X, Main.LocalPlayer.position.Y);
                Vector2 Offset = new(10, 40);
                Color Opacity = new(M.PotionOpacity, M.PotionOpacity, M.PotionOpacity, M.PotionOpacity);
                if (Main.LocalPlayer.GetModPlayer<Modplayer>().DebuffTimer > 0)     //Checking UI with isVisible doesn't work, sad
                {
                    if (M.BuffLayout == "Vertical")
                    {
                        Offset.X = -5f;
                    }
                    else
                    {
                        Offset.Y /= M.PotionScale;      //Negates multiplication in Draw
                        Offset.Y *= M.BuffScale;        //Makes pos equal to buff icons
                        Offset.Y += 30f;
                    }
                }
                if (Main.LocalPlayer.gravDir != 1f)
                {
                    Position.Y = (2 * Main.screenPosition.Y) + (float)Main.screenHeight - Position.Y;
                    Offset.Y += 40f / M.PotionScale;
                }
                spriteBatch.Draw(Request<Texture2D>("HPAware/UI/PotionReady").Value, Position - Main.screenPosition - (Offset * M.PotionScale), new Rectangle(0, 0, 28, 30), Opacity, 0f, Vector2.Zero, M.PotionScale, SpriteEffects.None, 1f);
            }
        }
    }
}
