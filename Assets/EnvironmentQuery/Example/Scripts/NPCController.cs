using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
	private NavMeshAgent agent;
	private EnvQuery query;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		query = GetComponent<EnvQuery>();
	}

	void Update()
	{
		if(query != null && query.BestResult != null)
		{
			agent.SetDestination(query.BestResult.GetWorldPosition());
		}		
	}
}
