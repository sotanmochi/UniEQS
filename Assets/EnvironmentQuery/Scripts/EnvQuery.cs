using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Environment Query Instance
 */
public class EnvQuery : MonoBehaviour
{
	public enum EnvQueryGeneratorType
	{
		OnCircle,
		SimpleGrid
	}

	public EnvQueryItem BestResult { get; private set; }

	public EnvQueryGeneratorType GeneratorType = EnvQueryGeneratorType.OnCircle;
	public GameObject CenterOfItems;
	public float Radius = 4.0f;
	public float SpaceBetween = 1.0f;
	public List<EnvQueryTest> EnvQueryTests = new List<EnvQueryTest>();

	private GameObject querier;
	private EnvQueryGenerator generator;
	private List<EnvQueryItem> envQueryItems;

	void Start()
	{
		if(querier == null)
		{
			querier = gameObject;
		}
		if(CenterOfItems == null)
		{
			CenterOfItems = querier;
		}

		if(GeneratorType == EnvQueryGeneratorType.OnCircle)
		{
			generator = new EnvQueryGeneratorOnCircle(Radius, SpaceBetween);
		}
		else if(GeneratorType == EnvQueryGeneratorType.SimpleGrid)
		{
			generator = new EnvQueryGeneratorSimpleGrid(Radius, SpaceBetween);
		}

		if(CenterOfItems != null && generator != null)
		{
			envQueryItems = generator.GenerateItems(EnvQueryTests.Count, CenterOfItems.transform);
		}
		else
		{
			envQueryItems = new List<EnvQueryItem>();
		}
	}

	void Update()
	{
		ResetScore();
		foreach(EnvQueryItem item in envQueryItems)
		{
			item.UpdateNavMeshProjection();
		}
		for(int currentTest = 0; currentTest < EnvQueryTests.Count; currentTest++)
		{
			EnvQueryTests[currentTest].RunTest(currentTest, envQueryItems);
			EnvQueryTests[currentTest].NormalizeItemScores(currentTest, envQueryItems);
		}
		FinalizeQuery();
	}

	private void ResetScore()
	{
		foreach(EnvQueryItem item in envQueryItems)
		{
			item.Score = 0.0f;
		}
	}

	private void FinalizeQuery()
	{
		NormalizeScore();
		BestResult = envQueryItems.Where(x => x.IsValid).OrderByDescending(x => x.Score).FirstOrDefault();
	}

	private void NormalizeScore()
	{
        if(envQueryItems == null || envQueryItems.Count < 1)
        {
            return;
        }

		float maxScore = envQueryItems[0].Score;
		float minScore = envQueryItems[0].Score;

		foreach(EnvQueryItem item in envQueryItems)
		{
			if(item.Score > maxScore)
			{
				maxScore = item.Score;
			}
			if(item.Score < minScore)
			{
				minScore = item.Score;
			}
		}

		if(maxScore != minScore)
		{
			foreach(EnvQueryItem item in envQueryItems)
			{
				item.Score = (item.Score - minScore) / (maxScore - minScore);
			}
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if(isActiveAndEnabled && envQueryItems != null)
		{
			foreach(EnvQueryItem item in envQueryItems)
			{
				if(item.IsValid)
				{
					Gizmos.color = Color.HSVToRGB((item.Score/2.0f), 1.0f, 1.0f);
					Gizmos.DrawWireSphere(item.GetWorldPosition(), 0.25f);
					UnityEditor.Handles.Label(item.GetWorldPosition(), item.Score.ToString());
				}
			}
		}
		if(isActiveAndEnabled && BestResult != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(BestResult.GetWorldPosition(), 0.25f);
		}
	}
#endif
}
