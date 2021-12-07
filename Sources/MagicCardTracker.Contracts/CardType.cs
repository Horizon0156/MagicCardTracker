using System;

namespace MagicCardTracker.Contracts
{
    /// <summary>
    ///     Enumeration for card types.
    /// </summary>
    public enum CardType
    {
        Creature,
        Planeswalker,
        Enchantment,
        Instant, 
        Sorcery,
        Artifact,
        Land,
        Other
    }
}
