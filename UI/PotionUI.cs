using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace HPAware.UI
{
    internal class PotionUI : UIState
    {
        private readonly Modconfig M = GetInstance<Modconfig>();

        private static Asset<Texture2D> Sprite;

        public override void OnActivate()
        {
            if (Sprite == null)
            {
                Sprite = Request<Texture2D>("HPAware/UI/PotionReady");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.dedServ && Main.myPlayer == Main.LocalPlayer.whoAmI)
            {
                Vector2 Position = new(Main.LocalPlayer.Center.X, Main.LocalPlayer.position.Y);
                Vector2 Offset = new(10, 40);
                Color Opacity = new(M.PotionOpacity, M.PotionOpacity, M.PotionOpacity, M.PotionOpacity);
                if (Main.LocalPlayer.GetModPlayer<Modplayer>().DebuffTimer > 0)     //Checks that debuff UI is active, checking UI with isVisible doesn't work, sad
                {
                    if (M.BuffLayout == "Vertical")
                    {
                        Offset.X = 35f;
                    }
                    else
                    {
                        Offset.Y /= M.PotionScale;      //Negates multiplication in Draw
                        Offset.Y *= M.BuffScale;        //Makes pos equal to buff icons
                        Offset.Y += (M.DisableBuffTimer || M.BuffLayout == "Most recent only") ? 30f : 30f + (25f * M.BuffScale);
                    }
                }
                if (Main.LocalPlayer.gravDir != 1f)
                {
                    Position.Y = (2 * Main.screenPosition.Y) + (float)Main.screenHeight - Position.Y;
                    Offset.Y += 40f / M.PotionScale;
                }
                spriteBatch.Draw(Sprite.Value, Position - Main.screenPosition - (Offset * M.PotionScale), new Rectangle(0, 0, 28, 30), Opacity, 0f, Vector2.Zero, M.PotionScale, SpriteEffects.None, 1f);
            }
        }
    }
}
