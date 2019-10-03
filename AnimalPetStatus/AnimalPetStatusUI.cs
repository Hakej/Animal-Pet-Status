using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace AnimalPetStatus
{
    public abstract class AnimalPetStatusUI
    {

        private static readonly Color BorderColor = new Color(133, 54, 5);
        private static readonly Color BorderFillColor = new Color(220, 123, 5);
        private static readonly Color BackgroundColor = new Color(245, 181, 101);
        private static readonly Color GradientStartColor = new Color(255, 197, 118);
        private static readonly Color GradientFinalColor = new Color(210, 140, 70);

        private static readonly SpriteFont Font = Game1.dialogueFont;

        private const int BorderSize = 4;

        private const int UIWidth = 280;
        private const int UIHeight = 45;

        public static void DrawUIWindow(SpriteBatch spriteBatch, Vector2 position, int width, int height)
        {
            var gradientTexture = Utilities.CreateGradientTexture(spriteBatch.GraphicsDevice, GradientStartColor, GradientFinalColor, width, height);

            var outerBorderRectangle = new Rectangle((int)position.X - BorderSize * 3, (int)position.Y - BorderSize * 3, width + BorderSize * 6, height + BorderSize * 6);
            var middleBorderRectangle = new Rectangle((int)position.X - BorderSize * 2, (int)position.Y - BorderSize * 2, width + BorderSize * 4, height + BorderSize * 4);
            var innerBorderRectangle = new Rectangle((int)position.X - BorderSize, (int)position.Y - BorderSize, width + BorderSize * 2, height + BorderSize * 2);
            var backgroundRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            spriteBatch.Draw(gradientTexture, outerBorderRectangle, BorderColor);
            spriteBatch.Draw(gradientTexture, middleBorderRectangle, BorderFillColor);
            spriteBatch.Draw(gradientTexture, innerBorderRectangle, BorderColor);
            spriteBatch.Draw(gradientTexture, backgroundRectangle, BackgroundColor);
        }

        public static void DrawMessage(SpriteBatch spriteBatch, Vector2 position, string message)
        {
            DrawUIWindow(spriteBatch, position, UIWidth, UIHeight);

            var textRectangle = new Rectangle((int)position.X + 10, (int)position.Y + 5, UIWidth - 20, UIHeight - 10);
            DrawStringFitInRectangle(spriteBatch, Font, message, textRectangle, Color.White, Color.Black, 1.5f);
        }

        public static void DrawAnimalNames(SpriteBatch spriteBatch, Vector2 position, IEnumerable<FarmAnimal> farmAnimals)
        {
            var totalHeight = UIHeight * farmAnimals.Count();
            DrawUIWindow(spriteBatch, position, UIWidth, totalHeight);

            var i = 0;
            foreach (var farmAnimal in farmAnimals)
            {
                var textRectangle = new Rectangle((int)position.X + 10, (int)position.Y + 5 + UIHeight * i, UIWidth - 20, UIHeight - 10);

                // If there is an animal above the current element of the list, draw a line between them
                if (i - 1 >= 0)
                {
                    DrawLine(spriteBatch, textRectangle, Color.Black, 1, UIWidth);
                }

                var message = farmAnimal.shortDisplayType() + " " + farmAnimal.Name;
                DrawStringFitInRectangle(spriteBatch, Font, message, textRectangle, Color.White, Color.Black, 1.5f);
                /*
            // TRIED TO DRAW A TEXTURE OF AN ANIMAL
            var texture = new Texture2D(spriteBatch.GraphicsDevice, 50, 50);
            Color[] colors = new Color[50 * 50];
            texture.SetData(colors);

            spriteBatch.Draw(texture, textRectangle, Color.White);
            */
                i++;
            }
        }

        /// <summary>
        /// Creates a pretty cool gradient texture!
        /// Used for a background Texture!
        /// </summary>
        /// <param name="graphicsDevice">The graphics device of the current viewport</param>
        /// <param name="width">The width of the current viewport</param>
        /// <param name="height">The height of the current viewport</param>
        /// A Texture2D with a gradient applied.

        private static void DrawLine(SpriteBatch spriteBatch, Rectangle boundaries, Color lineColor, int lineThickness, int lineWidth)
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
        public static void DrawStringFitInRectangle(SpriteBatch spriteBatch, SpriteFont font, string strToDraw, Rectangle boundaries, Color stringColor, Color stringShadowColor, float shadowScale)
        {
            var size = font.MeasureString(strToDraw);

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
            spriteBatch.DrawString(font, strToDraw, position + new Vector2(shadowScale * scale, shadowScale * scale), stringShadowColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
            spriteBatch.DrawString(font, strToDraw, position + new Vector2(-shadowScale * scale, shadowScale * scale), stringShadowColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
            spriteBatch.DrawString(font, strToDraw, position + new Vector2(-shadowScale * scale, -shadowScale * scale), stringShadowColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
            spriteBatch.DrawString(font, strToDraw, position + new Vector2(shadowScale * scale, -shadowScale * scale), stringShadowColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(font, strToDraw, position, stringColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        } // end DrawStringFitInRectangle()

        public static void DrawAnimalNamesInGame(IEnumerable<FarmAnimal> animals)
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
                DrawStringFitInRectangle(Game1.spriteBatch, Game1.tinyFont, animal.Name, textRectangle, color, Color.Black, 1.0f);
            }
        }
    }
}
