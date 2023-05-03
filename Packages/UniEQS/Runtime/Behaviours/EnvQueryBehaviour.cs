using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UniEQS.System;

namespace UniEQS.Behaviours
{
    public class EnvQueryBehaviour : MonoBehaviour
    {
        [SerializeField] private EnvQueryGeneratorBehaviour _generatorBehaviour;
        [SerializeField] private List<EnvQueryTestBehaviour> _queryTestBehaviours;
        
        public IEnvQuery EnvQuery => _envQuery;
        
        private IEnvQuery _envQuery;
        private List<EnvQueryTestBase> _queryTests = new List<EnvQueryTestBase>();
        
        void Start()
        {
            foreach (var queryTestBehaviour in _queryTestBehaviours)
            {
                _queryTests.Add(queryTestBehaviour.QueryTest);
            }
            
            _envQuery = new EnvQuery(_generatorBehaviour.Generator, _queryTests);
            _envQuery.Initialize();
        }
        
        void OnDestroy()
        {
            _envQuery.Dispose();
        }
        
        void Update()
        {
            _envQuery.Update();
        }
        
        void LateUpdate()
        {
            _envQuery.LateUpdate();
        }
        
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            DrawEnvQueryItemsInSceneView();
            DrawEnvQueryBestResultInSceneView();
        }
        
        void DrawEnvQueryItemsInSceneView()
        {
            if (isActiveAndEnabled && _envQuery?.EnvQueryItems != null)
            {
                foreach (EnvQueryItem item in _envQuery.EnvQueryItems)
                {
                    if (item.Enabled)
                    {
                        Gizmos.color = Color.HSVToRGB((item.Score / 2.0f), 1.0f, 1.0f);
                        Gizmos.DrawWireSphere(item.GetNavPosition(), 0.25f);
                        UnityEditor.Handles.Label(item.GetNavPosition(), item.Score.ToString());
                    }
                }
            }
        }
        
        void DrawEnvQueryBestResultInSceneView()
        {
            // if (isActiveAndEnabled && BestResult != null)
            // {
            //     Gizmos.color = Color.blue;
            //     Gizmos.DrawSphere(BestResult.GetWorldPosition(), 0.25f);
            // }
        }
#endif
    }
}