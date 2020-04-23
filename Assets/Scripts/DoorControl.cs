using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorControl : MonoBehaviour
{
	private BoxCollider2D Col;
	public GameObject EnterDialog;
	public string NextSceneName;
	private bool jumpPressed;
	// Start is called before the first frame update
	void Start()
	{
		Col = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		jumpPressed = FindObjectOfType<JumpEvent>().Getvalue();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")	
			EnterDialog.SetActive(true);
	}
	private void OnTriggerStay2D(Collider2D col)
	{
		if (col.tag == "Player" && jumpPressed == true)
		{
			if (FindObjectOfType<PlayerControl>().GetCherry() >= 5)
				SceneManager.LoadScene(NextSceneName);
		}
	}
	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player")
			EnterDialog.SetActive(false);
	}
}
