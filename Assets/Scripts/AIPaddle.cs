using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Rigidbody2D ball;
	[SerializeField] private float paddleSpeed = 1f;
	[SerializeField] private float reactionSmoothing = 1f;
	private float direction = 0f;
	[SerializeField] private AudioSource audio;
	[SerializeField] private AudioClip hitWall;
	[SerializeField] private LayerMask wall;

	private void Update()
	{
		float yDist = ball.transform.position.y - transform.position.y;
		yDist += ball.velocity.y * Time.deltaTime;
		float sign = Mathf.Sign(yDist);
		float absoluteDist = Mathf.Abs(yDist);
		direction = Mathf.Pow(Mathf.Clamp01(absoluteDist), reactionSmoothing) * sign;
	}

	private void FixedUpdate()
	{
		rb.velocity = Vector2.up * direction * paddleSpeed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject otherObj = collision.gameObject;
		int layer = 1 << otherObj.layer;
		if ((layer & wall) != 0)
		{
			audio.PlayOneShot(hitWall);
		}
	}
}
