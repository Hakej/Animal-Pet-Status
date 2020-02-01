using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace AnimalPetStatus
{
    public class Drawer
    {
        private static readonly Color BorderColor = new Color(133, 54, 5);
        private static readonly Color BorderFillColor = new Color(220, 123, 5);
        private static readonly Color BackgroundColor = new Color(245, 181, 101);
        private static readonly Color GradientStartColor = new Color(255, 197, 118);
        private static readonly Color GradientFinalColor = new Color(210, 140, 70);
        private static readonly SpriteFont Font = Game1.dialogueFont;

        private Vector2 _position;
        private int _width;
        private int _height;
        private int _borderSize;
        private Texture2D _gradientTexture;

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

        public Drawer(SpriteBatch spriteBatch)
        {
            _position = new Vector2(ModUI.X, ModUI.Y);
            _width = ModUI.Width;
            _height = ModUI.Height;
            _borderSize = ModUI.BorderSize;
        }

        public void UpdateGradient()
        {
            _gradientTexture = Utilities.CreateGradientTexture(Game1.spriteBatch.GraphicsDevice, GradientStartColor, GradientFinalColor, _width, _height);
        }

        public void DrawUIWindow()
        {
            var outerBorderRectangle = new Rectangle((int)_position.X - _borderSize * 3, (int)_position.Y - _borderSize * 3, _width + _borderSize * 6, _height + _borderSize * 6);
            var middleBorderRectangle = new Rectangle((int)_position.X - _borderSize * 2, (int)_position.Y - _borderSize * 2, _width + _borderSize * 4, _height + _borderSize * 4);
            var innerBorderRectangle = new Rectangle((int)_position.X - _borderSize, (int)_position.Y - _borderSize, _width + _borderSize * 2, _height + _borderSize * 2);
            var backgroundRectangle = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);

            Game1.spriteBatch.Draw(_gradientTexture, outerBorderRectangle, BorderColor);
            Game1.spriteBatch.Draw(_gradientTexture, middleBorderRectangle, BorderFillColor);
            Game1.spriteBatch.Draw(_gradientTexture, innerBorderRectangle, BorderColor);
            Game1.spriteBatch.Draw(_gradientTexture, backgroundRectangle, BackgroundColor);
        }

        public void DrawString(string message)
        {
            DrawUIWindow();
            var textRectangle = new Rectangle((int)_position.X + 10, (int)_position.Y + 5, _width - 20, _height - 10);
            DrawStringFitInRectangle(message, textRectangle);
        }

        internal void DrawUI(ObservableCollection<FarmAnimal> notPetAnimals)
        {
            if (!notPetAnimals.Any())
            {
                DrawString("No animal to pet.");
            }
            else
            {
                // Game1.drawWithBorder("dupa", Color.Red, Color.AliceBlue, ModUI.Position);
                // Game1.drawObjectDialogue("test");
                var animalNames = notPetAnimals.Select(farmAnimal => farmAnimal.Name);
                DrawStrings(_testListOfStrings);
            }
        }

        public void DrawStrings(IEnumerable<string> messages)
        {
            _height = _height * messages.Count();

            DrawUIWindow();

            var i = 0;
            foreach (var message in messages)
            {
                var textRectangle = new Rectangle((int)_position.X + 10, (int)_position.Y + 5 + _height * i, _width - 20, _height - 10);

                // If there is another element before the current element of the list, draw a line between them
                if (i - 1 >= 0)
                {
                    DrawLine(Game1.spriteBatch, textRectangle, Color.Black, 1, _width);
                }
                DrawStringFitInRectangle(message, textRectangle);
                i++;
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Rectangle boundaries, Color lineColor, int lineThickness, int lineWidth)
        {
            var lineTexture = new Texture2D(spriteBatch.GraphicsDevice, lineWidth, lineThickness);
            var colors = new Color[lineWidth];
            for (var j = 0; j < lineWidth; j++)
            {
                colors[j] = Color.White;
            }
            lineTexture.SetData(colors);

            var lineRectangle = boundaries;
            lineRectangle.X -= 10;
            lineRectangle.Y -= 5;
            lineRectangle.Width += 20;
            lineRectangle.Height = lineThickness;

            spriteBatch.Draw(lineTexture, lineRectangle, lineColor);
        }

        /// <summary>
        /// Draws the given string as large as possible inside the boundaries Rectangle without going
        /// outside of it.  This is accomplished by scaling the string (since the SpriteFont has a specific
        /// size).
        /// 
        /// If the string is not a perfect match inside of the boundaries (which it would rarely be), then
        /// the string will be absolutely-centered inside of the boundaries.
        /// </summary>
        public void DrawStringFitInRectangle(string strToDraw, Rectangle boundaries)
        {
            var size = Font.MeasureString(strToDraw);
            var color = Color.White;
            var shadowColor = Color.Black;
            var shadowScale = 1.5f;

            var xScale = (boundaries.Width / size.X);
            var yScale = (boundaries.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            var scale = Math.Min(xScale, yScale);

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            var strWidth = (int)Math.Round(size.X * scale);
            var strHeight = (int)Math.Round(size.Y * scale);
            var position = new Vector2
            {
                X = (((boundaries.Width - strWidth) / 2) + boundaries.X),
                Y = (((boundaries.Height - strHeight) / 2) + boundaries.Y)
            };

            // A bunch of settings where we just want to use reasonable defaults.
            const float rotation = 0.0f;
            var spriteOrigin = new Vector2(0, 0);
            const float spriteLayer = 0.0f;
            const SpriteEffects spriteEffects = new SpriteEffects();

            // Draw shadow to the string
            Game1.spriteBatch.DrawString(Font, strToDraw, position + new Vector2(shadowScale * scale, shadowScale * scale), shadowColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
            Game1.spriteBatch.DrawString(Font, strToDraw, position + new Vector2(-shadowScale * scale, shadowScale * scale), shadowColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
            Game1.spriteBatch.DrawString(Font, strToDraw, position + new Vector2(-shadowScale * scale, -shadowScale * scale), shadowColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
            Game1.spriteBatch.DrawString(Font, strToDraw, position + new Vector2(shadowScale * scale, -shadowScale * scale), shadowColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);

            // Draw the string to the sprite batch
            Game1.spriteBatch.DrawString(Font, strToDraw, position, color, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }

        public void DrawAnimalNamesInGame(IEnumerable<FarmAnimal> animals)
        {
            foreach (var animal in animals)
            {
                if (!animal.currentLocation.Equals(Game1.player.currentLocation)) continue;
                var x = animal.Position.X + (float)animal.Sprite.getWidth() / 2;
                var y = animal.Position.Y - (float)Game1.tileSize * 4 / 3;
                var position = Game1.GlobalToLocal(Game1.viewport, new Vector2(x, y));
                const int width = 100;
                const int height = 30;
                var textRectangle = new Rectangle((int)position.X - 25, (int)position.Y + 135, width, height);

                var color = animal.wasPet.Value ? Color.Green : Color.White;
                DrawStringFitInRectangle(animal.Name, textRectangle);
            }
        }
    }
}
