using IMLibrary.Data;
using System.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary.Helpers
{
    public interface IRuntimeConfig
    {
        Dictionary<string, string> GetConfigDictionary();
    }
    public class RuntimeConfig : IRuntimeConfig
    {
        private readonly IM_API_Context _context;

        public RuntimeConfig(IM_API_Context context)
        {
            _context = context;
        }

        public Dictionary<string, string> GetConfigDictionary()
        {
            var cached = MemoryCache.Default.Get("RuntimeConfig");
            if (cached != null)
            {
                return (Dictionary<string, string>)cached;
            }

            var dict = new Dictionary<string, string>();
            foreach (var rci in _context.RuntimeConfigItems)
            {
                dict.Add(rci.Key, rci.Value);
            }

            MemoryCache.Default.Set("RuntimeConfig", dict, DateTimeOffset.Now.AddMinutes(15));
            return dict;
        }
    }
}
