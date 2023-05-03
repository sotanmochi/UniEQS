using System.Collections.Generic;

namespace UniEQS
{
    public interface IEnvQueryGenerator
    {
        List<EnvQueryItem> GenerateItems(int numberOfQueryTests);
    }
}