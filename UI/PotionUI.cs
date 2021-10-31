using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace HPAware.UI
{
    internal class PotionUI : UIState
    {
        private UIImage PotionReady;

        public override void OnActivate()
        {
            Texture2D texture = ModContent.Request<Texture2D>("HPAware/UI/PotionReady").Value;
            PotionReady = new UIImage(texture);
            Append(PotionReady);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 Position = Main.LocalPlayer.position - Main.screenPosition;
            MarginLeft = Position.X;
            MarginTop = Position.Y - 65;
            Recalculate();
        }
    }
}
