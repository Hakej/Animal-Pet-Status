using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace AnimalPetStatus
{
    /// <summary>The mod entry point.</summary>
    public class AnimalPetStatus : Mod
    {
        public static Configuration Config;

        private bool _petHudEnabled = true;
        private bool _allPetsPetHUDMessageShown;

        private IEnumerable<FarmAnimal> _allAnimals;
        private IEnumerable<FarmAnimal> _animalsToPet;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<Configuration>();

            helper.Events.Display.RenderedHud += Display_RenderedHud;
            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void Display_RenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (!_petHudEnabled)
                return;

            if (!_animalsToPet.Any())
            {
                Drawer.DrawString(e.SpriteBatch, "No animal to pet.", ModUI.Position, ModUI.Width, ModUI.Height, ModUI.BorderSize);
            }
            else
            {
                var animalNames = _animalsToPet.Select(farmAnimal => farmAnimal.Name);
                Drawer.DrawStrings(e.SpriteBatch, animalNames, ModUI.Position, ModUI.Width, ModUI.Height, ModUI.BorderSize);
            }

            Drawer.DrawAnimalNamesInGame(_allAnimals);
        }

        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            switch (e.Button)
            {
                case ModConstants.ToggleButton:
                    _petHudEnabled = !_petHudEnabled;
                    break;
                case SButton.MouseRight:
                    UpdateAnimalsToPet();
                    break;
            }
        }

        private void UpdateAnimalsToPet()
        {
            _allAnimals = Game1.getFarm().getAllFarmAnimals();

            _animalsToPet = _allAnimals.Where(animal => !animal.wasPet.Value);

            if (_animalsToPet.Any())
            {
                _allPetsPetHUDMessageShown = false;
                return;
            }

            if (_allPetsPetHUDMessageShown)
                return;

            _allPetsPetHUDMessageShown = true;
            Game1.addHUDMessage(new HUDMessage("All pets have been pet! :)", 4));
            PlayAllPetsPetJingle();
        }

        private static void PlayAllPetsPetJingle()
        {
            Game1.playSound("drumkit4");
            DelayedAction.playSoundAfterDelay("drumkit1", 100);
            DelayedAction.playSoundAfterDelay("drumkit2", 200);
            DelayedAction.playSoundAfterDelay("Duck", 200);
        }
    }
}