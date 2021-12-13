using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent;
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
                Vector2 Position = new(Main.LocalPlayer.Center.X, Main.LocalPlayer.position.Y);
                Vector2 Offset = new(15, 40);
                Color Opacity = new(M.BuffOpacity, M.BuffOpacity, M.BuffOpacity, M.BuffOpacity);
                if (Main.LocalPlayer.GetModPlayer<Modplayer>().PotionTimer > 0 && M.BuffLayout == "Vertical")     //Checking UI with isVisible doesn't work, sad
                {
                    Offset.X = 35f;
                }
                if (Main.LocalPlayer.gravDir != 1f)
                {
                    Position.Y = (2 * Main.screenPosition.Y) + (float)Main.screenHeight - Position.Y;
                    Offset.Y += 40f / M.BuffScale;
                }
                if (M.BuffLayout != "Most recent only")
                {
                    float Temp = Offset.Y;
                    for (int i = 0; i < Main.LocalPlayer.GetModPlayer<Modplayer>().DebuffsToShow.Count; i++)
                    {
                        if (M.BuffLayout == "Vertical")
                        {
                            Offset.Y = 30f * (Main.LocalPlayer.GetModPlayer<Modplayer>().DebuffsToShow.Count - 1);
                            Offset.Y += i * -30f;
                            Offset.Y += Temp;
                        }
                        else
                        {
                            Offset.X = (i * -30f) + (Main.LocalPlayer.GetModPlayer<Modplayer>().DebuffsToShow.Count * 15);
                        }
                        spriteBatch.Draw(TextureAssets.Buff[Main.LocalPlayer.GetModPlayer<Modplayer>().DebuffsToShow[i]].Value, Position - Main.screenPosition - (Offset * M.BuffScale), new Rectangle(0, 0, 32, 32), Opacity, 0f, Vector2.Zero, M.BuffScale, SpriteEffects.None, 1f);
                    }
                }
                else
                {
                    spriteBatch.Draw(TextureAssets.Buff[Main.LocalPlayer.GetModPlayer<Modplayer>().DebuffToShow].Value, Position - Main.screenPosition - (Offset * M.BuffScale), new Rectangle(0, 0, 32, 32), Opacity, 0f, Vector2.Zero, M.BuffScale, SpriteEffects.None, 1f);
                }
            }
        }
    }
}
