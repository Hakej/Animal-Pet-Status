using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalPetStatus
{
    public abstract class Utilities
    {
        public static Texture2D CreateGradientTexture(GraphicsDevice graphicsDevice, Color startColor, Color finalColor, int width, int height)
        {
            var gradientTexture = new Texture2D(graphicsDevice, width, height);
            var colors = new Color[height * width];

            // Create gradient colors 
            for (var i = 0; i < colors.Length; i++)
            {
                // value between 0 and 1.0 indicating the weight of finalColor
                var colorAmount = (float)i / colors.Length;
                colors[i] = Color.Lerp(startColor, finalColor, colorAmount);
            }
            gradientTexture.SetData(colors);

            return gradientTexture;
        }
    }
}
