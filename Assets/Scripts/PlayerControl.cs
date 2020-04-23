using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	private Rigidbody2D Body;
	private Animator Anime;
	public LayerMask Ground;
	private BoxCollider2D BoxCol;
	private CircleCollider2D CircleCol;

	public AudioSource BGMAudio, DeadAudio, JumpAudio, HurtAudio, CollectAudio;
	public float Speed = 10.0f, JumpForce = 13.0f;
	public Text CherryNumber;
	private int count = 0;

	public Transform ceilCheck, groundCheck;

	[SerializeField]
	private bool isGround, isCeil;
	private bool isHurt, isJump, isCrouch;
	private int jumpCount;
	private bool jumpPressed, jumpOut, crouchPressed;

	public Joystick joyStick;
	// Start is called before the first frame update
	void Start()
	{

		Body = GetComponent<Rigidbody2D>();
		Anime = GetComponent<Animator>();
		BoxCol = GetComponent<BoxCollider2D>();
		CircleCol = GetComponent<CircleCollider2D>();
	}

	void Update()
	{
		jumpPressed = FindObjectOfType<JumpEvent>().Getvalue();
		crouchPressed = FindObjectOfType<CrouchEvent>().Getvalue();
		if (jumpPressed == false) jumpOut = true; 
		CherryNumber.text = count.ToString();
		if (jumpPressed == true && jumpOut == true && jumpCount > 0 && !isCrouch && !isHurt)
			{ isJump = true; jumpOut = false; }
		if (crouchPressed && isGround && !isHurt) isCrouch = true;
		if (!crouchPressed && !isCeil && !isHurt) isCrouch = false;
	}


	void FixedUpdate()
	{
		isCeil = Physics2D.OverlapCircle(ceilCheck.position, 0.1f, Ground);
		isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, Ground);
		if (!isHurt) Move();
		Jump();
		Crouch();
		SwitchAnime();
	}

	int FaceDirection = 1;
	void Move()
	{
		float HorizontalMove = joyStick.Horizontal;
		Body.velocity = new Vector2(HorizontalMove * Speed, Body.velocity.y);
		if (HorizontalMove > 0) FaceDirection = 1;
		else if (HorizontalMove < 0) FaceDirection = -1;
		transform.localScale = new Vector3(FaceDirection, 1, 1);
	}

	void Jump()
	{
		if (isGround) jumpCount = 2;
		if (isJump && jumpCount > 0)
		{
			JumpAudio.Play();
			Body.velocity = new Vector2(Body.velocity.x, JumpForce);
			jumpCount--;
			isJump = false;
		}
	}

	void Crouch()
	{
		BoxCol.enabled = !isCrouch;
	}

	void SwitchAnime()
	{
		Anime.SetFloat("Running", Mathf.Abs(Body.velocity.x)); //Runnning
		if (!isGround && Body.velocity.y > 0.1f) Anime.SetBool("Jumping", true); //Jump
		if (isGround) { Anime.SetBool("Falling", false); Anime.SetBool("Jumping", false); }
		else if (Body.velocity.y < -0.1f)
		{
			Anime.SetBool("Falling", true);
			Anime.SetBool("Jumping", false);
		}//Fall
		if (isHurt) Anime.SetBool("Hurting", true);
		if (isHurt && Mathf.Abs(Body.velocity.x) < 0.1f)
		{
			isHurt = false;
			Anime.SetBool("Hurting", false);
			Anime.SetFloat("Running", 0.0f);
		}//Jump
		Anime.SetBool("Crouching", isCrouch); //crouch
	}

	void ReStart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void CherryCount() {count++;}
	public int GetCherry() {return count;}
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Collections")
		{
			CollectAudio.Play();
			col.GetComponent<Animator>().Play("Got");
		}
		if (col.tag == "Deadline")
		{
			BGMAudio.enabled = false;
			DeadAudio.enabled = true;
			Invoke("ReStart", 1.0f);
		}
	}//collect & die

	private void OnCollisionEnter2D(Collision2D col)
	{
		FrogContorl frog = col.gameObject.GetComponent<FrogContorl>();

		if (col.gameObject.tag == "Enemies")
		{
			if (Anime.GetBool("Falling") == true)
			{
				frog.Death();
				Body.velocity = new Vector2(Body.velocity.x, JumpForce);
				Anime.SetBool("Jumping", true);
			}
			else
			{
				isHurt = true;
				HurtAudio.Play();
				if (transform.position.x < col.gameObject.transform.position.x)
					Body.velocity = new Vector2(-5.0f, Body.velocity.y);
				if (transform.position.x > col.gameObject.transform.position.x)
					Body.velocity = new Vector2(5.0f, Body.velocity.y);
			}
		}
	}//touch enemies

}
