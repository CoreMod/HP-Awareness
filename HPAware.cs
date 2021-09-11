using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using HPAware.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace HPAware
{
	public class HPAware : Mod
	{
        
    }

    public class HPAwareSystem : ModSystem
    {
        internal DebuffUI DebuffState;  //original
        internal UserInterface DebuffInterface;  //clone
        internal PotionUI PotionState;
        internal UserInterface PotionInterface;
        private GameTime lastUpdateUiGameTime;
        internal HPBarUI HPBarState;
        internal UserInterface HPBarInterface;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                Filters.Scene["HPOverlay"] = new Filter(new ScreenShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/HPOverlay", AssetRequestMode.ImmediateLoad).Value), "HPOverlay"), EffectPriority.VeryHigh);
                Filters.Scene["HPOverlay2"] = new Filter(new ScreenShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/HPOverlay", AssetRequestMode.ImmediateLoad).Value), "HPOverlay2"), EffectPriority.VeryHigh);
                Filters.Scene["NewHPOverlay"] = new Filter(new ScreenShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/NewHPOverlay", AssetRequestMode.ImmediateLoad).Value), "NewHPOverlay"), EffectPriority.VeryHigh);
                Filters.Scene["NewHPOverlay2"] = new Filter(new ScreenShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/NewHPOverlay", AssetRequestMode.ImmediateLoad).Value), "NewHPOverlay2"), EffectPriority.VeryHigh);
                Filters.Scene["HPOverlayFlat"] = new Filter(new ScreenShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/HPOverlayFlat", AssetRequestMode.ImmediateLoad).Value), "HPOverlayFlat"), EffectPriority.VeryHigh);

                DebuffInterface = new UserInterface();
                DebuffState = new DebuffUI();
                PotionInterface = new UserInterface();
                PotionState = new PotionUI();
                HPBarInterface = new UserInterface();
                HPBarState = new HPBarUI();
                PotionState.Activate();
            }
        }

        public override void PreSaveAndQuit()
        {
            HideDebuff();
            HidePotion();
            HideHPBar();    //HP bar persists on rejoin, this fixes it
        }
        //Used for UI activation and deactivation
        internal void ShowDebuff()
        {
            DebuffInterface?.SetState(DebuffState);
        }
        internal void ShowPotion()
        {
            PotionInterface?.SetState(PotionState);
        }
        internal void ShowHPBar()
        {
            HPBarInterface?.SetState(HPBarState);
        }
        internal void HideDebuff()
        {
            DebuffInterface?.SetState(null);
        }
        internal void HidePotion()
        {
            PotionInterface?.SetState(null);
        }
        internal void HideHPBar()
        {
            HPBarInterface?.SetState(null);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            //Update UI if it is on
            lastUpdateUiGameTime = gameTime;
            if (DebuffInterface?.CurrentState != null)
            {
                DebuffInterface.Update(gameTime);
            }
            if (PotionInterface?.CurrentState != null)
            {
                PotionInterface.Update(gameTime);
            }
            if (HPBarInterface?.CurrentState != null)
            {
                HPBarInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //Draw UI underneath minimap layer when UI is on
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Map / Minimap"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer
                (
                    "HP Awareness: Potion Pop Up",
                    delegate
                    {
                        if (lastUpdateUiGameTime != null && PotionInterface?.CurrentState != null)
                        {
                            PotionInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);
                        }
                        return true;
                    },
                InterfaceScaleType.Game
                ));
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer
                (
                    "HP Awareness: Debuff Pop Up",
                    delegate
                    {
                        if (lastUpdateUiGameTime != null && DebuffInterface?.CurrentState != null)
                        {
                            DebuffInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);
                        }
                        return true;
                    },
                InterfaceScaleType.Game
                ));
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer
                (
                    "HP Awareness: Player HP Bar",
                    delegate
                    {
                        if (lastUpdateUiGameTime != null && HPBarInterface?.CurrentState != null)
                        {
                            HPBarInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);
                        }
                        return true;
                    },
                InterfaceScaleType.Game
                ));
            }
        }
    }
}