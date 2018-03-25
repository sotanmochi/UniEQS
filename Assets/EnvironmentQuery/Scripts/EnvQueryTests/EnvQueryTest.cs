using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvQueryTest : ScriptableObject
{
    public enum EnvQueryTestScoringEquation
    {
        Linear,
        InverseLinear,
        Square,
        HalfSine,
        InverseHalfSine,
        HalfSineSquared,
        InverseHalfSineSquared,
        SigmoidLike,
        InverseSigmoidLike
    }

    public bool IsActive;
    public float ScaleFactor;
    public EnvQueryTestScoringEquation ScoringEquation;

    public EnvQueryTest()
    {
        IsActive = true;
        ScaleFactor = 1.0f;
        ScoringEquation = EnvQueryTestScoringEquation.Linear;
    }

    public virtual void RunTest(int currentTest, List<EnvQueryItem> envQueryItems){}

    public void NormalizeItemScores(int currentTest, List<EnvQueryItem> envQueryItems)
    {
        if(envQueryItems == null || envQueryItems.Count < 1)
        {
            return;
        }

		float maxValue = envQueryItems[0].TestResults[currentTest];
		float minValue = envQueryItems[0].TestResults[currentTest];

		foreach(EnvQueryItem item in envQueryItems)
		{
            if(item.IsValid)
            {
                float value = item.TestResults[currentTest];
                if(value > maxValue)
                {
                    maxValue = value;
                }
                if(value < minValue)
                {
                    minValue = value;
                }
            }
		}

        if(maxValue != minValue)
        {
            foreach(EnvQueryItem item in envQueryItems)
            {
                if(!item.IsValid)
                {
                    continue;
                }

                float weightedScore = 0.0f;
                float normalizedScore = (item.TestResults[currentTest] - minValue) / (maxValue - minValue);

                switch(ScoringEquation)
                {
                    case EnvQueryTestScoringEquation.Linear:
                        weightedScore = ScaleFactor * normalizedScore;
                        break;
                    case EnvQueryTestScoringEquation.InverseLinear:
                        weightedScore = ScaleFactor * (1.0f - normalizedScore);
                        break;
                    case EnvQueryTestScoringEquation.Square:
                        weightedScore = ScaleFactor * (normalizedScore * normalizedScore);
                        break;
                    case EnvQueryTestScoringEquation.HalfSine:
                        weightedScore = ScaleFactor * Mathf.Sin(Mathf.PI * normalizedScore);
                        break;
                    case EnvQueryTestScoringEquation.InverseHalfSine:
                        weightedScore = ScaleFactor * -Mathf.Sin(Mathf.PI * normalizedScore);
                        break;
                    case EnvQueryTestScoringEquation.HalfSineSquared:
                        weightedScore = ScaleFactor * Mathf.Sin(Mathf.PI * normalizedScore) * Mathf.Sin(Mathf.PI * normalizedScore);
                        break;
                    case EnvQueryTestScoringEquation.InverseHalfSineSquared:
                        weightedScore = ScaleFactor * -(Mathf.Sin(Mathf.PI * normalizedScore) * Mathf.Sin(Mathf.PI * normalizedScore));
                        break;
                    case EnvQueryTestScoringEquation.SigmoidLike:
                        weightedScore = ScaleFactor * (((float)Math.Tanh( 4.0f * (normalizedScore - 0.5f) ) + 1.0f) / 2.0f);
                        break;
                    case EnvQueryTestScoringEquation.InverseSigmoidLike:
                        weightedScore = ScaleFactor * ( 1.0f - (((float)Math.Tanh( 4.0f * (normalizedScore - 0.5f) ) + 1.0f) / 2.0f) );
                        break;
                    default:
                        break;
                }

                item.Score += weightedScore;
            }
        }
    }
}
