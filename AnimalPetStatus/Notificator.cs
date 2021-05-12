using StardewValley;

namespace AnimalPetStatus
{
    public class Notificator
    {
        public static void NotifyWithJingle()
        {
            Game1.addHUDMessage(new HUDMessage("All pets have been pet! :)", HUDMessage.newQuest_type));
            PlayJingle();
        }

        private static void PlayJingle()
        {
            DelayedAction.playSoundAfterDelay("drumkit4", 0);
            DelayedAction.playSoundAfterDelay("drumkit1", 200);
            DelayedAction.playSoundAfterDelay("drumkit2", 400);
            DelayedAction.playSoundAfterDelay("Duck", 575);
        }
    }
}
