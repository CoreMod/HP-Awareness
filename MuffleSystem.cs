using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace HPAware
{
    public class MuffleSystem : ModPlayer
    {
        private readonly Modconfig M = GetInstance<Modconfig>();

        public override void PostUpdateMiscEffects()
        {
            MuffleMusic();
        }

        public override void UpdateAutopause()
        {
            //Also run while game is paused (Note: seems musicFade resets to 1 if refocusing game but whatever)
            MuffleMusic();
        }

        private void MuffleMusic()
        {
            if (!Main.dedServ && Main.myPlayer == Player.whoAmI)
            {
                //musicFade is used by ML pre-spawn (for 1 frame) and transitioning between music tracks
                //It ranges 0-1 regardless of music volume
                if (Player.statLife <= Player.statLifeMax2 * M.MuffleTrigger &&
                Main.curMusic > 0 && Main.curMusic < Main.musicFade.Length)
                {
                    if (Main.musicFade[Main.curMusic] > 1f - M.MuffleIntensity)
                    {
                        Main.musicFade[Main.curMusic] -= 0.01f;
                    }
                    else
                    {
                        Main.musicFade[Main.curMusic] = 1f - M.MuffleIntensity;
                    }
                }
            }
        }
    }
}