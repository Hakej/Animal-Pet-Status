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

        private const int MULTIPLE_STRINGS_OFFSET = 25;

        public Drawer(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
        }

        public void DrawString(string stringToDraw, Vector2 position)
        {
            _spriteBatch.DrawString(_spriteFont, stringToDraw, position, Color.White);
        }

        public void DrawStrings(IEnumerable<string> stringsToDraw, Vector2 position)
        {
            var localPosition = position;

            foreach (var s in stringsToDraw)
            {
                _spriteBatch.DrawString(_spriteFont, s, localPosition, Color.White);

                localPosition.Y += MULTIPLE_STRINGS_OFFSET;
            }
        }
    }
}
