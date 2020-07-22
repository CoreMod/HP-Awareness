using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace HPAware
{
    public class Modplayer : ModPlayer
    {
        private bool Overlay;
        private float Counter;

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (!GetInstance<Modconfig>().DisableHurtOverlay)
            {
                Overlay = true;
                Counter = 30;
            }           
        }

        public override void UpdateBiomeVisuals()
        {
            if (!Main.dedServ && Overlay)
            {
                Filters.Scene.Activate("HPOverlay");               
            }
        }

        public override void PostUpdate()
        {
            if (Filters.Scene["HPOverlay"].IsActive())
            {
                Filters.Scene["HPOverlay"].GetShader().UseOpacity(Counter / 10);
                Counter--;
                
                if (Counter == 0)
                {
                    Filters.Scene["HPOverlay"].Deactivate();
                    Overlay = false;
                }
            }                 

            if (!Main.dedServ && player.statLife <= player.statLifeMax2 * GetInstance<Modconfig>().Overlaytrigger && !GetInstance<Modconfig>().DisableLowHpOverlay)
            {
                Filters.Scene.Activate("HPOverlay2");
            }
            else
            {
                Filters.Scene["HPOverlay2"].Deactivate();
            }

            if (Filters.Scene["MoonLord"].IsActive() && GetInstance<Modconfig>().DisableMLShader)
            {
                Filters.Scene["MoonLord"].Deactivate();
            }
        }
    }
}