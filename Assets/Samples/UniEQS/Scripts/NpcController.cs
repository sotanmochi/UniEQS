using UnityEngine;
using UnityEngine.AI;
using UniEQS.Behaviours;

namespace UniEQS.Samples
{
    public class NpcController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnvQueryBehaviour _envQueryBehaviour;
        
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        void Update()
        {
            var envQuery = _envQueryBehaviour.EnvQuery;
            if (envQuery != null && envQuery.BestResult != null)
            {
                _agent.SetDestination(envQuery.BestResult.GetNavPosition());
            }
        }
    }
}