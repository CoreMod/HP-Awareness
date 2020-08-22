using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using static Terraria.ModLoader.ModContent;

namespace HPAware.UI
{
    internal class PotionUI : UIState
    {
        private UIImage PotionReady;

        public override void OnActivate()
        {
            Texture2D texture = GetTexture("HPAware/UI/PotionReady");
            PotionReady = new UIImage(texture);
            Append(PotionReady);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 Position = new Vector2(Main.LocalPlayer.position.X - Main.screenPosition.X, Main.LocalPlayer.position.Y - Main.screenPosition.Y);
            MarginLeft = Position.X;
            MarginTop = Position.Y - 65;
            Recalculate();
        }
    }
}
