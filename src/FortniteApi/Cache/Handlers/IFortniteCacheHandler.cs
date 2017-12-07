namespace FortniteApi.Cache.Handlers
{
    public interface IFortniteCacheHandler
    {
        /// <summary>
        ///     Loads the cache.
        /// </summary>
        /// <param name="email">The email of the current account. It is hashed and used for the filename.</param>
        /// <returns>Returns the cache from disk or a new cache if the cache was not found.</returns>
        FortniteCache Load(string email);

        /// <summary>
        ///     Saves the cache to the disk.
        /// </summary>
        /// <param name="email">The email of the current account. It is hashed and used for the filename.</param>
        /// <param name="cache">The cache that has to be saved.</param>
        void Save(string email, FortniteCache cache);
    }
}
