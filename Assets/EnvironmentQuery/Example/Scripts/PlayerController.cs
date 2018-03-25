using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 3.5f;
	public float rotationSpeed = 180f;

	private CharacterController characterController;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
	}
	
	void Update()
	{
		Vector3 direction = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;		
		if(direction.sqrMagnitude > 0.01f)
		{
			Vector3 forward = Vector3.Slerp(
				transform.forward,
				direction,
				rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction)
			);
			transform.LookAt(transform.position + forward);
		}
		characterController.Move(direction * moveSpeed * Time.deltaTime);
	}
}
