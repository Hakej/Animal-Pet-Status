using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace AnimalPetStatus
{
    /// <summary>The mod entry point.</summary>
    public class AnimalPetStatus : Mod
    {

        private const SButton ToggleButton = SButton.O;

        private const int _UIX = 20;
        private const int _UIY = 20;
        private static readonly Vector2 _UIPosition = new Vector2(_UIX, _UIY);

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
            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            helper.Events.Display.RenderingHud += Display_RenderingHud;
            helper.Events.Display.RenderedHud += Display_RenderedHud;
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void Display_RenderingHud(object sender, RenderingHudEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (!_petHudEnabled)
                return;

            AnimalPetStatusUI.DrawAnimalNamesInGame(_allAnimals);
        }

        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == ToggleButton)
            {
                _petHudEnabled = !_petHudEnabled;
            }
        }

        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            
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

        private void Display_RenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (!_petHudEnabled)
                return;

            if (!_animalsToPet.Any())
            {
                AnimalPetStatusUI.DrawMessage(e.SpriteBatch, _UIPosition, "No animal to pet.");
            }
            else
            {
                AnimalPetStatusUI.DrawAnimalNames(e.SpriteBatch, _UIPosition, _animalsToPet);
            }
        }
    }
}