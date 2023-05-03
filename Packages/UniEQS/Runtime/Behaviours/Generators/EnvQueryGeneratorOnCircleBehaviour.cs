using UnityEngine;
using UniEQS.Generators;

namespace UniEQS.Behaviours.Generators
{
    public class EnvQueryGeneratorOnCircleBehaviour : EnvQueryGeneratorBehaviour
    {
        [SerializeField] private EnvQueryGeneratorOnCircle _generator;
        public override IEnvQueryGenerator Generator => _generator;
    }
}