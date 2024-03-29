﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace HPAware.UI
{
    internal class HPBarUI : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
			//Code directly from vanilla but modified slightly
			if (!Main.dedServ && Main.myPlayer == Main.LocalPlayer.whoAmI)
			{
				float HPPercent = (float)Main.LocalPlayer.statLife / (float)Main.LocalPlayer.statLifeMax2;
				if (HPPercent > 1f)
				{
					HPPercent = 1f;
				}
				int HP36 = (int)(36f * HPPercent);
				float Scale = GetInstance<Modconfig>().HPBarScale;
				float X = Main.LocalPlayer.Center.X - (18f * Scale);
				float Y = Main.LocalPlayer.Center.Y + Main.LocalPlayer.height - 10f;
				if (Main.LocalPlayer.gravDir != 1f)     //Upside down
				{
					Y -= Main.LocalPlayer.height + 20f;         //Upside down offset for player
					Y = (2 * Main.screenPosition.Y) + (float)Main.screenHeight - Y;     //Double scrnPos negates the one used in draw code, screenHeight = resolution
				}
				float R;
				float G;
				float B = 0f;
				float A = 255f;
				HPPercent -= 0.1f;
				if (HPPercent > 0.5f)
				{
					G = 255f;
					R = 255f * (1f - HPPercent) * 2f;
				}
				else
				{
					G = 255f * HPPercent * 2f;
					R = 255f;
				}
				R *= Main.LocalPlayer.GetModPlayer<Modplayer>().BarAlpha;
				G *= Main.LocalPlayer.GetModPlayer<Modplayer>().BarAlpha;
				A *= Main.LocalPlayer.GetModPlayer<Modplayer>().BarAlpha;
				R = MathHelper.Clamp(R, 0f, 255f);
				G = MathHelper.Clamp(G, 0f, 255f);
				A = MathHelper.Clamp(A, 0f, 255f);
				Color color = new((byte)R, (byte)G, (byte)B, (byte)A);
				if (HP36 < 3)
				{
					HP36 = 3;
				}
				if (HP36 < 34)  // Slightly less than full HP
				{
					//HP border (inbetween filled and empty bar)
					spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(X - Main.screenPosition.X + (float)HP36 * Scale, Y - Main.screenPosition.Y), new Rectangle(2, 0, 2, TextureAssets.Hb2.Height()), color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
					//HP BG (empty)
					spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(X - Main.screenPosition.X + (float)(HP36 + 2) * Scale, Y - Main.screenPosition.Y), new Rectangle(HP36 + 2, 0, 36 - HP36 - 2, TextureAssets.Hb2.Height()), color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
					//HP bar (filled)
					spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(X - Main.screenPosition.X, Y - Main.screenPosition.Y), new Rectangle(0, 0, HP36 - 2, TextureAssets.Hb1.Height()), color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
					//HP border 2
					spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(X - Main.screenPosition.X + (float)(HP36 - 2) * Scale, Y - Main.screenPosition.Y), new Rectangle(32, 0, 2, TextureAssets.Hb1.Height()), color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				}
				else
				{
					if (HP36 < 36)  //HP Edge
					{
						spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(X - Main.screenPosition.X + (float)HP36 * Scale, Y - Main.screenPosition.Y), new Rectangle(HP36, 0, 36 - HP36, TextureAssets.Hb2.Height()), color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
					}
					//Entire HP Bar
					spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(X - Main.screenPosition.X, Y - Main.screenPosition.Y), new Rectangle(0, 0, HP36, TextureAssets.Hb1.Height()), color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				}
			}
		}
    }
}
