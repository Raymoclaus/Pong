using UnityEngine;

public class PaddleInput : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private float paddleSpeed;
	private float direction = 0f;
	[SerializeField] private AudioSource audio;
	[SerializeField] private AudioClip hitWall;
	[SerializeField] private LayerMask wall;

	private void Update()
	{
		direction = Input.GetAxisRaw("Vertical");
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
