using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using NoAlloq;
using UniEQS.QueryTests;
using UniEQS.Utility;

namespace UniEQS.System
{   
    public class EnvQuery : IEnvQuery
    {
        public List<EnvQueryItem> EnvQueryItems => _envQueryItems;
        public EnvQueryItem BestResult { get; private set; }
        
        private int _jobBatchSize = 32;
        private readonly IEnvQueryGenerator _generator;
        private readonly List<EnvQueryTestBase> _envQueryTests;
        
        private List<EnvQueryItem> _envQueryItems;
        private List<EnvQueryItem> _envQueryItemsBacking;
        
        private List<NativeArray<EnvQueryTestResult>> _testResults;
        private NativeArray<EnvQueryTestItem> _queryTestItems;
        private NativeList<JobHandle> _queryTestJobHandles;
        
        // Environment query tests
        private List<EnvQueryDistanceTestJob> _distanceTestJobs = new List<EnvQueryDistanceTestJob>();
        private List<EnvQueryDotTestJob> _dotTestJobs = new List<EnvQueryDotTestJob>();
        
        public EnvQuery
        (
            IEnvQueryGenerator generator,
            List<EnvQueryTestBase> envQueryTests
        )
        {
            _generator = generator;
            _envQueryTests = envQueryTests;
        }
        
        public void Initialize()
        {
            _envQueryItems = _generator.GenerateItems(_envQueryTests.Count);
            _envQueryItemsBacking = _envQueryItems.GetRange(0, _envQueryItems.Count);
            
            _queryTestItems = new NativeArray<EnvQueryTestItem>(_envQueryItems.Count, Allocator.Persistent);

            // var queryTest
            var distanceTestCount = 0;
            var dotTestCount = 0;
            for (var i = 0; i < _envQueryTests.Count; i++)
            {
                var queryTest = _envQueryTests[i];
                
                if (queryTest is EnvQueryDistanceTest distanceTest)
                {
                    _distanceTestJobs.Add(new EnvQueryDistanceTestJob());
                    distanceTestCount++;
                }
                else 
                if (queryTest is EnvQueryDotTest dotTest)
                {
                    _dotTestJobs.Add(new EnvQueryDotTestJob());
                    dotTestCount++;
                }
            }
            
            var queryTestCount = distanceTestCount + dotTestCount;

            _queryTestJobHandles = new NativeList<JobHandle>(queryTestCount, Allocator.Persistent);
            for (var i = 0; i < queryTestCount; i++)
            {
                _queryTestJobHandles.Add(default);
            }
            
            _testResults = new List<NativeArray<EnvQueryTestResult>>();
            for (var i = 0; i < queryTestCount; i++)
            {
                _testResults.Add(new NativeArray<EnvQueryTestResult>(_queryTestItems.Length, Allocator.Persistent));
            }
        }
        
        public void Dispose()
        {
            for (var i = 0; i < _testResults.Count; i++)
            {
                _testResults[i].Dispose();
            }
            _queryTestItems.Dispose();
            _queryTestJobHandles.Dispose();
        }
        
        public void Update()
        {
            UpdateQueryTestItems();
            ScheduleQueryTestJobs();
            JobHandle.ScheduleBatchedJobs();
        }
        
        public void LateUpdate()
        {
            CompleteQueryTestJobs();
            NormalizeItemScores();
            UpdateScore();
        }
        
        public void UpdateQueryTestItems()
        {
            for (var i = 0; i < _envQueryItems.Count; i++)
            {
    			_envQueryItems[i].UpdateNavMeshProjection();
                _queryTestItems[i] = new EnvQueryTestItem()
                {
                    WorldPosition = _envQueryItems[i].GetNavPosition()
                };
            }
        }
        
        public void ScheduleQueryTestJobs()
        {
            var distanceTestCount = 0;
            var dotTestCount = 0;
            
            for (var i = 0; i < _envQueryTests.Count; i++)
            {
                var queryTest = _envQueryTests[i];
                
                if (queryTest is EnvQueryDistanceTest distanceTest)
                {
                    _distanceTestJobs[distanceTestCount++] = new EnvQueryDistanceTestJob()
                    {
                        Enabled = distanceTest.Enabled,
                        Items = _queryTestItems,
                        DistanceToPosition = distanceTest.DistanceToPosition,
                    };
                }
                else
                if (queryTest is EnvQueryDotTest dotTest)
                {
                    _dotTestJobs[dotTestCount++] = new EnvQueryDotTestJob()
                    {
                        Enabled = dotTest.Enabled,
                        Items = _queryTestItems,
                        AbsoluteValue = dotTest.AbsoluteValue,
                        DirectionA = dotTest.DirectionA,
                        TargetPosition = dotTest.TargetPosition,
                        DirectionB = dotTest.DirectionB,
                        ForwardDirection = dotTest.ForwardDirection,
                        RightDirection = dotTest.RightDirection,
                        LineFromPosition = dotTest.LineFromPosition,
                        LineToPosition = dotTest.LineToPostision,
                    };
                }
            }
            
            var jobCount = 0;
            for (var i = 0; i < _distanceTestJobs.Count; i++)
            {
                var distanceTestJob = _distanceTestJobs[i];
                distanceTestJob.Results = _testResults[jobCount];
                _distanceTestJobs[i] = distanceTestJob;
                
                _queryTestJobHandles[jobCount++] = distanceTestJob.Schedule(_envQueryItems.Count(), _jobBatchSize);
            }
            for (var i = 0; i < _dotTestJobs.Count; i++)
            {
                var dotTestJob = _dotTestJobs[i];
                dotTestJob.Results = _testResults[jobCount];
                _dotTestJobs[i] = dotTestJob;
                
                _queryTestJobHandles[jobCount++] = dotTestJob.Schedule(_envQueryItems.Count(), _jobBatchSize);
            }            
        }
        
        public void CompleteQueryTestJobs()
        {
            JobHandle.CompleteAll(_queryTestJobHandles);
        }
        
        public void NormalizeItemScores()
        {
            for (var testIndex = 0; testIndex < _testResults.Count; testIndex++)
            {
                var results = _testResults[testIndex];
                
                var maxValue = results[0].Value;
                var minValue = results[0].Value;
                
                for (var itemIndex = 0; itemIndex < results.Length; itemIndex++)
                {
                    var value = results[itemIndex].Value;
                    if (value > maxValue) { maxValue = value; }
                    if (value < minValue) { minValue = value; }
                }
                
                if (!maxValue.Equals(minValue))
                {
                    for (var itemIndex = 0; itemIndex < results.Length; itemIndex++)
                    {
                        var weightedScore = 0f;
                        var normalizedScore = (results[itemIndex].Value - minValue) / (maxValue - minValue);
                        
                        weightedScore = normalizedScore; // Linear and ScaleFactor = 1.0f
                        
                        // TODO:
                        // switch (scoringEquation)
                        // {
                        //     case EnvQueryTestScoringEquation.Linear:
                        //         weightedScore = ScaleFactor * normalizedScore;
                        //         break;
                        //     case EnvQueryTestScoringEquation.InverseLinear:
                        //         weightedScore = ScaleFactor * (1.0f - normalizedScore);
                        //         break;
                        //     case EnvQueryTestScoringEquation.Square:
                        //         weightedScore = ScaleFactor * (normalizedScore * normalizedScore);
                        //         break;
                        //     case EnvQueryTestScoringEquation.HalfSine:
                        //         weightedScore = ScaleFactor * Mathf.Sin(Mathf.PI * normalizedScore);
                        //         break;
                        //     case EnvQueryTestScoringEquation.InverseHalfSine:
                        //         weightedScore = ScaleFactor * -Mathf.Sin(Mathf.PI * normalizedScore);
                        //         break;
                        //     case EnvQueryTestScoringEquation.HalfSineSquared:
                        //         weightedScore = ScaleFactor * Mathf.Sin(Mathf.PI * normalizedScore) * Mathf.Sin(Mathf.PI * normalizedScore);
                        //         break;
                        //     case EnvQueryTestScoringEquation.InverseHalfSineSquared:
                        //         weightedScore = ScaleFactor * -(Mathf.Sin(Mathf.PI * normalizedScore) * Mathf.Sin(Mathf.PI * normalizedScore));
                        //         break;
                        //     case EnvQueryTestScoringEquation.SigmoidLike:
                        //         weightedScore = ScaleFactor * (((float)Math.Tanh( 4.0f * (normalizedScore - 0.5f) ) + 1.0f) / 2.0f);
                        //         break;
                        //     case EnvQueryTestScoringEquation.InverseSigmoidLike:
                        //         weightedScore = ScaleFactor * ( 1.0f - (((float)Math.Tanh( 4.0f * (normalizedScore - 0.5f) ) + 1.0f) / 2.0f) );
                        //         break;
                        //     default:
                        //         break;
                        // }

                        var testResult = results[itemIndex];
                        testResult.Value = weightedScore;
                        results[itemIndex] = testResult;
                    }
                }
            }
        }
        
        public void UpdateScore()
        {
            foreach (var envQueryItem in _envQueryItems)
            {
                envQueryItem.Score = 0.0f;
            }
            
            for (var testIndex = 0; testIndex < _testResults.Count; testIndex++)
            {
                var results = _testResults[testIndex];

                for (var itemIndex = 0; itemIndex < results.Length; itemIndex++)
                {
                    _envQueryItems[itemIndex].Score += results[itemIndex].Value;
                }
            }
            
            BestResult = _envQueryItems
                            .AsSpan()
                            .Where(x => x.Enabled)
                            .OrderByDescending(_envQueryItemsBacking.AsSpan(), x => x.Score)
                            .FirstOrDefault();
        }
    }
}