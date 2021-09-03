namespace MagicCardTracker.Contracts
{
    /// <summary>
    ///     Enumeration for supported update modes
    /// </summary>
    public enum UpdateMode
    {
        /// <summary> Updates the prices of the collection </summary>
        Prices,
        /// <summary> Updates all mutable properties the collection </summary>
        AllMutableProperties
    }
}
