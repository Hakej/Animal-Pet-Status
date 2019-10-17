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
        private ObservableCollection<FarmAnimal> _animalsToPet = new ObservableCollection<FarmAnimal>();

        private readonly List<string> _testListOfStrings = new List<string>
        {
            "Dupa",
            "Cyce",
            "Wadowice",
            "Polska",
            "Niemcy",
            "Chuj",
            "Rewolwer",
            "Mizina",
            "Jebać Jackala",
            "Elo",
            "Mordo",
            "1337"
        };

    /*********
    ** Public methods
    *********/
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<Configuration>();

            _animalsToPet.CollectionChanged += _animalsToPet_CollectionChanged;

            helper.Events.Display.RenderedHud += Display_RenderedHud;
            helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
        }

        private void _animalsToPet_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!e.NewItems.Count.Equals(0)) return;

            this.Monitor.Log("Collection has been changed:" + e.NewItems.Count, LogLevel.Info);

            Game1.addHUDMessage(new HUDMessage("All pets have been pet! :)", 4));
            PlayAllPetsPetJingle();
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
                // Game1.drawWithBorder("dupa", Color.Red, Color.AliceBlue, ModUI.Position);
                Game1.drawObjectDialogue("test");
                var animalNames = _animalsToPet.Select(farmAnimal => farmAnimal.Name);
                // Drawer.DrawStrings(e.SpriteBatch, _testListOfStrings, ModUI.Position, ModUI.Width, ModUI.Height, ModUI.BorderSize);
            }

            Drawer.DrawAnimalNamesInGame(_allAnimals);
        }
        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            UpdateAnimalsToPet();
        }

        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button.IsActionButton())
                UpdateAnimalsToPet();

            switch (e.Button)
            {
                case ModConstants.ToggleButton:
                    _petHudEnabled = !_petHudEnabled;
                    break;
                case SButton.P:
                    Game1.drawDialogue(Game1.getCharacterFromName("Marnie"), "no elo kurwa jak tam twoja krowa zrogowaciala");
                    break;
            }
        }

        private void UpdateAnimalsToPet()
        {
            var allAnimalsList = Game1.getFarm().getAllFarmAnimals();
            _allAnimals = new ObservableCollection<FarmAnimal>(allAnimalsList);
            var animalsToPetList = _allAnimals.Where(animal => !animal.wasPet.Value);
            _animalsToPet = new ObservableCollection<FarmAnimal>(animalsToPetList);
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