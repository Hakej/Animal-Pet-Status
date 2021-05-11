﻿using System;
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
        // SHOW SETTINGS
        public bool Show = true;
        public SButton ToggleButton = SButton.P;

        // NEEDED CLASSES
        public Drawer Drawer;

        // TEXT
        public Vector2 Position = new Vector2(10, 10);
        public const int MULTIPLE_STRINGS_OFFSET = 30;

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

            var farmAnimals = Game1.getFarm().getAllFarmAnimals()
                .Where(a => !a.wasPet)
                .Select(a => a.Name);

            Drawer.DrawStringsWithBackground(farmAnimals, Position, BackgroundTop, BackgroundMiddle, BackgroundBottom);
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