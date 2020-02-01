using StardewValley;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalPetStatus
{
    public static class ModEvents
    {
        public static void AnimalsToPetUpdated(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!e.NewItems.Count.Equals(0)) return;

            Game1.addHUDMessage(new HUDMessage("All pets have been pet! :)", 4));
            Utilities.PlayAllPetsPetJingle();
        }
    }
}
