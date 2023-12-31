﻿using Mossad_Recruitment.Api.Infrastructure.Services.Interfaces;

namespace Mossad_Recruitment.Api.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly Dictionary<string, object> _internalStorage = new();

        public T Get<T>(string key)
        {
            return _internalStorage.ContainsKey(key) ? (T)_internalStorage[key] : default(T);
        }

        public void Set<T>(string key, T value)
        {
            _internalStorage[key] = value;
        }
    }
}
