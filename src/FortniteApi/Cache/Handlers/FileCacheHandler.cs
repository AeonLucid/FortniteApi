using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FortniteApi.Response.Persona;
using Newtonsoft.Json;

namespace FortniteApi.Cache.Handlers
{
    /// <summary>
    ///     Caches API stuff to the disk.
    /// </summary>
    public class FileCacheHandler : IFortniteCacheHandler
    {
        private readonly string _cacheDirectory;

        /// <summary>
        ///     Caches API stuff to the disk.
        /// </summary>
        /// <param name="cacheDirectory">The directory where cache files will be stored.</param>
        public FileCacheHandler(string cacheDirectory)
        {
            _cacheDirectory = cacheDirectory;

            Directory.CreateDirectory(_cacheDirectory);
        }

        /// <inheritdoc />
        public FortniteCache Load(string email)
        {
            var fileName = GetFileName(email);

            if (!File.Exists(fileName))
            {
                return new FortniteCache();
            }

            return JsonConvert.DeserializeObject<FortniteCache>(File.ReadAllText(fileName));
        }

        /// <inheritdoc />
        public void Save(string email, FortniteCache cache)
        {
            var fileName = GetFileName(email);

            // Not interested in storing the lookup cache to the disk.
            var cloneCache = new FortniteCache
            {
                LookupCache = new Dictionary<string, LookupResponse>(),
                FortniteToken = cache.FortniteToken
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(cloneCache, Formatting.Indented));
        }

        private string GetFileName(string email)
        {
            var hash = Hash(email);
            var fileName = $"{hash}.json";

            return Path.Combine(_cacheDirectory, fileName);
        }

        private static string Hash(string input)
        {
            using (var hash = new SHA1CryptoServiceProvider())
            {
                var bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(input));

                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }
    }
}
