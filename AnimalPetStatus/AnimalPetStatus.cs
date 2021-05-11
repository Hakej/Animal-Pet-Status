using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace AnimalPetStatus
{
    public class AnimalPetStatus : Mod
    {
        // MOD SETTINGS
        public bool Show = true;
        public SButton ToggleButton = SButton.P;
        public Vector2 Position = new Vector2(10, 10);

        // TEXT
        public Color textColor = Color.Black;

        // NEEDED CLASSES
        public Drawer Drawer;

        // BACKGROUND
        public Texture2D BackgroundTop;
        public Texture2D BackgroundMiddle;
        public Texture2D BackgroundBottom;

        public override void Entry(IModHelper helper)
        {
            BackgroundTop = helper.Content.Load<Texture2D>("Assets/background_top.png", ContentSource.ModFolder);
            BackgroundMiddle = helper.Content.Load<Texture2D>("Assets/background_middle.png", ContentSource.ModFolder);
            BackgroundBottom = helper.Content.Load<Texture2D>("Assets/background_bottom.png", ContentSource.ModFolder);

            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.Display.RenderedHud += OnRenderedHud;
            helper.Events.GameLoop.GameLaunched += GameLaunched;
        }

        private void GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Drawer = new Drawer(Game1.spriteBatch, Game1.smallFont);
        }

        private void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (!Show)
                return;

            var notPetAnimals = Game1.getFarm().getAllFarmAnimals()
                .Where(a => !a.wasPet)
                .OrderBy(a => a.Name);

            //Drawer.DrawStringsWithBackground(notPetAnimals, Position, textColor, BackgroundTop, BackgroundMiddle, BackgroundBottom);
            Drawer.DrawAnimalNamesWithBackground(notPetAnimals, Position, BackgroundTop, BackgroundMiddle, BackgroundBottom);
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == ToggleButton)
                Show = !Show;
        }
    }
}