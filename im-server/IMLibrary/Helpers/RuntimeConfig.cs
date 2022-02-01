using IMLibrary.Data;
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
            var dict = new Dictionary<string, string>();
            foreach (var rci in _context.RuntimeConfigItems)
            {
                dict.Add(rci.Key, rci.Value);
            }
            return dict;
        }
    }
}
