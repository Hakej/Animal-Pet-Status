﻿using System;
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
        public bool Show = true;
        public SButton ToggleButton = SButton.P;

        // TEXT
        public Vector2 Position = new Vector2(10, 10);

        public Drawer Drawer;

        public override void Entry(IModHelper helper)
        {
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

            string[] strings = { "One", "Two", "Three" };

            Drawer.DrawStrings(strings, Position);
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == ToggleButton)
                Show = !Show;

            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }
    }
}