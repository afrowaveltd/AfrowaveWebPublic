using Microsoft.Extensions.Caching.Distributed;

namespace Id.I18n
{
    public class JsonStringLocalizerFactory(IDistributedCache cache) : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cache = cache;

		public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(_cache);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(_cache);
        }
    }
}