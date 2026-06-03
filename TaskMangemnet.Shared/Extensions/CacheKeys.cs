using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMangemnet.Shared.Extensions
{
    public static class CacheKeys
    {
        public static string GenerateCacheKeyForTask(Guid taskId) => $"task:{taskId}";
    }
}
