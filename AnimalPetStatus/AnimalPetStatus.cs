using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    // TODO:
    // BUG: Jingle doesn't play when petting the last animal to pet.
    // OPTIMIZE: Drawing the UI makes freeze the game each second.

    /// <summary>The mod entry point.</summary>
    public class AnimalPetStatus : Mod
    {
        public static Configuration Config;

        private bool _petHudEnabled = true;
        private ObservableCollection<FarmAnimal> _allAnimals = new ObservableCollection<FarmAnimal>();
        private ObservableCollection<FarmAnimal> _notPetAnimals = new ObservableCollection<FarmAnimal>();
        private Drawer _drawer;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<Configuration>();
            _drawer = new Drawer(Game1.spriteBatch);

            // Listen for custom events
            _notPetAnimals.CollectionChanged += ModEvents.AnimalsToPetUpdated;

            // Listen for default events
            helper.Events.Display.RenderedHud += Display_RenderedHud;
            helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
        }

        private void Display_RenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (!_petHudEnabled)
                return;

            _drawer.UpdateGradient();
            _drawer.DrawUI(_notPetAnimals);
            _drawer.DrawAnimalNamesInGame(_allAnimals);
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            UpdateAnimals();
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button.IsActionButton())
                UpdateAnimals();

            switch (e.Button)
            {
                case ModConstants.ToggleButton:
                    _petHudEnabled = !_petHudEnabled;
                    break;
            }
        }

        private void UpdateAnimals()
        {
            var allAnimals = Game1.getFarm().getAllFarmAnimals();
            var notPetAnimals = _allAnimals.Where(animal => !animal.wasPet.Value);

            _allAnimals = new ObservableCollection<FarmAnimal>(allAnimals);
            _notPetAnimals = new ObservableCollection<FarmAnimal>(notPetAnimals);
        }
    }
}