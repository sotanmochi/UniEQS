using System;
using System.Collections.Generic;

namespace UniEQS
{
    public interface IEnvQuery : IDisposable
    {
        EnvQueryItem BestResult { get; }
        List<EnvQueryItem> EnvQueryItems { get; }
        void Initialize();
        void Update();
        void LateUpdate();
    }
}