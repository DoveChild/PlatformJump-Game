using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public Transform Camera;
	public float MoveRate = 0.0f;
	private float StartPoint;
	// Start is called before the first frame update
	void Start()
	{
		StartPoint = transform.position.x;
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = new Vector2(StartPoint + Camera.position.x * MoveRate, transform.position.y);
	}
}
