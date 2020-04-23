using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogContorl : MonoBehaviour
{
	private Rigidbody2D Body;
	private Animator Anime;
	private int FaceDirection = -1;
	private CircleCollider2D Col;
	public LayerMask Ground;
	private float Speed = 4.0f, JumpForce = 6.0f;
	public Transform Left, Right;
	private float leftpoint, rightpoint;

	private AudioSource DeathAudio;

	// Start is called before the first frame update
	void Start()
    {
		Body = GetComponent<Rigidbody2D>();
		Anime = GetComponent<Animator>();
		Col = GetComponent<CircleCollider2D>();
		DeathAudio = GetComponent<AudioSource>();
		leftpoint = Left.transform.position.x; //Destroy(Left.gameObject);
		rightpoint = Right.transform.position.x; //Destroy(Right.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
		SwitchAnime();
    }

	private int times = 0;
	void Move()
	{
		if (FaceDirection == -1 && transform.position.x <= leftpoint) FaceDirection = 1;
		else if (FaceDirection == 1 && transform.position.x >= rightpoint) FaceDirection = -1;
		if (times >= 3) { FaceDirection *= -FaceDirection; times = 0; }
		transform.localScale = new Vector3(-FaceDirection, 1, 1);


		Body.velocity = new Vector2((float)FaceDirection * Speed, JumpForce);
		Anime.SetBool("Jumping", true);
		times++;
	}

	void SwitchAnime()
	{
		if (Col.IsTouchingLayers(Ground) == true && Anime.GetBool("Falling") == true)
			Anime.SetBool("Falling", false);
		else if (Anime.GetBool("Jumping") == true && Body.velocity.y < 0.0f)
		{
			Anime.SetBool("Jumping", false);
			Anime.SetBool("Falling", true);
		}
	}

	public void Death()
	{
		Col.enabled = false;
		DeathAudio.Play();
		Anime.SetTrigger("Dying");
	}
	void Destroy()
	{
		Destroy(gameObject);
	}

}
