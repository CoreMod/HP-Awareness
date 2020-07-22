using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;


namespace HPAware
{
	public class HPAware : Mod
	{
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Filters.Scene["HPOverlay"] = new Filter(new ScreenShaderData(new Ref<Effect>(GetEffect("Effects/HPOverlay")), "HPOverlay"), EffectPriority.VeryHigh);
                Filters.Scene["HPOverlay2"] = new Filter(new ScreenShaderData(new Ref<Effect>(GetEffect("Effects/HPOverlay")), "HPOverlay2"), EffectPriority.VeryHigh);              
            }
        }
    }
}