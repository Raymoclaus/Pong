using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private float startSpeed;
	[SerializeField] private float startMaxAngle = 0f;
	[SerializeField] private LayerMask paddle, playerSideGoal, enemySideGoal;
	[SerializeField] private float speedIncreasePerHit = 0.1f;
	[SerializeField] private float paddleReboundAngleLimit = 75f;
	private float currentSpeed = 0f;
	[SerializeField] private ScoreTracker playerScore, enemyScore;
	[SerializeField] private AudioSource audio;
	[SerializeField] private AudioClip hitWall, hitPlayerSideGoal, hitEnemySideGoal;
	[SerializeField] private LayerMask wall;

	private void Start()
	{
		Reset();
	}

	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		GameObject otherObj = otherCollider.gameObject;
		int layer = 1 << otherObj.layer;
		if ((layer & paddle) != 0)
		{
			currentSpeed += speedIncreasePerHit;

			Vector2 directionToPaddleCenter = otherObj.transform.position - transform.position;
			float signedAngle = Vector2.SignedAngle(Vector2.up, -directionToPaddleCenter);
			float sign = Mathf.Sign(signedAngle);
			float angle = Vector2.Angle(Vector2.up, -directionToPaddleCenter);
			float clampedAngle = Mathf.Clamp(angle, 90f - paddleReboundAngleLimit, 90f + paddleReboundAngleLimit);
			float finalAngle = clampedAngle * -sign * Mathf.Deg2Rad;
			Vector2 newDirection = new Vector2(Mathf.Sin(finalAngle), Mathf.Cos(finalAngle));
			newDirection = ScaleToXSpeed(newDirection, currentSpeed);

			rb.velocity = newDirection;
		}

		if ((layer & playerSideGoal) != 0)
		{
			enemyScore.IncrementScore();
			audio.PlayOneShot(hitPlayerSideGoal);
			Reset();
		}

		if ((layer & enemySideGoal) != 0)
		{
			playerScore.IncrementScore();
			audio.PlayOneShot(hitEnemySideGoal);
			Reset();
		}
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

	private void Reset()
	{
		transform.position = Vector2.zero;
		float defaultStartingAngle = 90f;
		float adjustedStartingAngle = defaultStartingAngle + Random.value * startMaxAngle * 2f - startMaxAngle;
		float angleRadians = adjustedStartingAngle * Mathf.Deg2Rad;
		currentSpeed = startSpeed;
		Vector2 direction = new Vector2(Mathf.Sin(angleRadians), Mathf.Cos(angleRadians));
		direction = ScaleToXSpeed(direction, currentSpeed);
		rb.velocity = direction;
	}

	private Vector2 ScaleToXSpeed(Vector2 direction, float xSpeed)
	{
		float currentRatio = xSpeed / Mathf.Abs(direction.x);
		return direction * currentRatio;
	}

	private Vector2 CurrentVelocity => rb.velocity;

	private Vector2 CurrentDirection => CurrentVelocity.normalized;
}
