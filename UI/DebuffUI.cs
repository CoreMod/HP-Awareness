using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
using ReLogic.Graphics;

namespace HPAware.UI
{
    internal class DebuffUI : UIState
    {
        readonly Modconfig M = GetInstance<Modconfig>();

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.dedServ && Main.myPlayer == Main.LocalPlayer.whoAmI)
            {
                Modplayer P = Main.LocalPlayer.GetModPlayer<Modplayer>();
                Vector2 Position = new(Main.LocalPlayer.Center.X, Main.LocalPlayer.position.Y);
                Vector2 Offset = new(15, 40);
                Color Opacity = new(M.BuffOpacity, M.BuffOpacity, M.BuffOpacity, M.BuffOpacity);
                if (P.PotionTimer > 0 && M.BuffLayout == "Vertical")     //Checks that potion UI is active, checking UI with isVisible doesn't work, sad
                {
                    Offset.X = -5f;
                }
                if (Main.LocalPlayer.gravDir != 1f)
                {
                    Position.Y = (2 * Main.screenPosition.Y) + (float)Main.screenHeight - Position.Y;
                    Offset.Y += 40f / M.BuffScale;
                }
                Vector2 RealPos = Position - Main.screenPosition - (Offset * M.BuffScale);
                if (M.BuffLayout != "Most recent only")
                {
                    float Temp = Offset.Y;
                    for (int i = 0; i < P.DebuffsToShow.Count; i++)
                    {
                        if (M.BuffLayout == "Vertical")
                        {
                            Offset.Y = 30f * (P.DebuffsToShow.Count - 1);
                            Offset.Y += i * -30f;
                            Offset.Y += Temp;
                        }
                        else
                        {
                            Offset.X = (i * -30f) + (P.DebuffsToShow.Count * 15);
                        }
                        RealPos = Position - Main.screenPosition - (Offset * M.BuffScale);      //Apply new offset
                        spriteBatch.Draw(TextureAssets.Buff[P.DebuffsToShow[i]].Value, RealPos, new Rectangle(0, 0, 32, 32), Opacity, 0f, Vector2.Zero, M.BuffScale, SpriteEffects.None, 1f);
                        if (!M.DisableBuffTimer)
                        {
                            Vector2 Side = (M.BuffLayout == "Vertical") ? new Vector2(35f, 8f) * M.BuffScale : new Vector2(2f, -14f - (i % 2 * 15f)) * M.BuffScale;     //Text offset
                            CalculateAndDrawTime(spriteBatch, P.DebuffsToShow[i], RealPos + Side);
                        }
                    }
                }
                else if (Main.LocalPlayer.HasBuff(P.DebuffToShow))
                {
                    spriteBatch.Draw(TextureAssets.Buff[P.DebuffToShow].Value, RealPos, new Rectangle(0, 0, 32, 32), Opacity, 0f, Vector2.Zero, M.BuffScale, SpriteEffects.None, 1f);
                    if (!M.DisableBuffTimer)
                    {
                        CalculateAndDrawTime(spriteBatch, P.DebuffToShow, RealPos + new Vector2(35f * M.BuffScale, 8f * M.BuffScale));
                    }
                }
            }
        }

        private void CalculateAndDrawTime(SpriteBatch spriteBatch, int Type, Vector2 Position)
        {
            if (Main.buffNoTimeDisplay[Type])
            {
                return;
            }
            int Index = Main.LocalPlayer.FindBuffIndex(Type);
            if (Index == -1)        //No longer has buff
            {
                return;
            }
            int DebuffTime = Main.LocalPlayer.buffTime[Index];
            int DrawTime = DebuffTime;
            string Unit = " s";
            if (DebuffTime > 60)        //Seconds
            {
                DrawTime /= 60;
            }
            if (DebuffTime > 3600)      //Minutes
            {
                DrawTime /= 60;
                DrawTime += 1;          //Rounding up
                Unit = " m";
            }
            if (DebuffTime > 216000)    //Hours
            {
                DrawTime /= 60;
                Unit = " h";
            }
            if (DebuffTime <= 60)
            {
                DrawTime = 0;
            }
            Color Opacity = new(M.BuffOpacity, M.BuffOpacity, M.BuffOpacity, M.BuffOpacity);
            DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, DrawTime + Unit, Position, Opacity, 0f, Vector2.Zero, M.BuffScale - 0.2f, SpriteEffects.None, 1f);
        }
    }
}
