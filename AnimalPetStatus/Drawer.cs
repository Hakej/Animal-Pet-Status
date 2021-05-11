using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalPetStatus
{
    public class Drawer
    {
        private static SpriteBatch _spriteBatch;
        private static SpriteFont _spriteFont;

        public Drawer(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
        }

        public void DrawString(string stringToDraw, Vector2 position)
        {
            _spriteBatch.DrawString(_spriteFont, stringToDraw, position, Color.White);
        }

        public void DrawStrings(IEnumerable<string> stringsToDraw, Vector2 position, int offset)
        {
            var localRectangle = new Rectangle((int)position.X, (int)position.Y, 160, 25);

            foreach (var s in stringsToDraw)
            {
                DrawStringAligned(s, localRectangle, Alignment.Center);

                localRectangle.Y += offset;
            }
        }

        public void DrawStringsWithBackground(IEnumerable<string> stringsToDraw, Vector2 position, Texture2D backgroundTop, Texture2D backgroundMiddle, Texture2D backgroundBottom)
        {
            _spriteBatch.Draw(backgroundTop, position, Color.White);

            var middlePosition = position;
            middlePosition.Y += backgroundTop.Height;

            var drawingRectangle = new Rectangle((int)middlePosition.X, (int)middlePosition.Y, backgroundMiddle.Width, backgroundMiddle.Height);

            foreach (var s in stringsToDraw)
            {
                _spriteBatch.Draw(backgroundMiddle, drawingRectangle, Color.White);

                DrawStringAligned(s, drawingRectangle, Alignment.Center);

                drawingRectangle.Y += backgroundMiddle.Height;
            }

            _spriteBatch.Draw(backgroundBottom, drawingRectangle, Color.White);
        }

        [Flags]
        public enum Alignment { Center = 0, Left = 1, Right = 2, Top = 4, Bottom = 8 }

        private void DrawStringAligned(string text, Rectangle bounds, Alignment align)
        {
            Vector2 size = _spriteFont.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X, bounds.Center.Y);
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(Alignment.Left))
                origin.X += bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Right))
                origin.X -= bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Top))
                origin.Y += bounds.Height / 2 - size.Y / 2;

            if (align.HasFlag(Alignment.Bottom))
                origin.Y -= bounds.Height / 2 - size.Y / 2;

            _spriteBatch.DrawString(_spriteFont, text, pos, Color.White, 0, origin, 1, SpriteEffects.None, 0);
        }
    }
}
