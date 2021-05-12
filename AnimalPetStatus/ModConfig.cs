using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace AnimalPetStatus
{
    public class ModConfig
    {
        public Vector2 Position { get; set; } = new Vector2(10, 10);
        public bool IsActive { get; set; } = true;
        public SButton ToggleButton = SButton.P;
        public SButton MoveButton = SButton.L;
    }
}
