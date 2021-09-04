using System;

namespace MagicCardTracker.Contracts
{
    /// <summary>
    ///     Enumeration for card legalities.
    /// </summary>
    [Flags]
    public enum Legality
    {
        Standard = 1,
        Modern = 2,
        Pioneer = 4,
        Legacy = 8, 
        Vintage = 16,
        Brawl = 32,
        Historic = 64, 
        Pauper = 128,
        Penny = 256,
        Commander = 512
    }
}
