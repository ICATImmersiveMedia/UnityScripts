using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFloatEffect : MonoBehaviour {

	private static Vector3 normal = new Vector3 (1, 0, 0);

	public float startRadius;
	public float gravityFactor;
	public float initialTangentialVelocity;

	private Vector2 focal;
	private Vector2 velocity;

	void Start() {
		focal = new Vector2 (transform.position.z, transform.position.y);

		float startAngle = Random.Range (0, 2 * Mathf.PI);

		Vector3 tempPos = transform.position;
		tempPos.z += startRadius * Mathf.Cos (startAngle);
		tempPos.y += startRadius * Mathf.Sin (startAngle);
		transform.position = tempPos;

		Vector3 tangentialVelocity = Vector3.Cross (new Vector3 (0, focal.y - transform.position.y, focal.x - transform.position.z), normal);
		velocity = tangentialVelocity.normalized * initialTangentialVelocity;
	}

	void Update () {
		Vector2 acceleration = new Vector2 (focal.x - transform.position.z, focal.y - transform.position.y).normalized * gravityFactor;
		velocity += acceleration;

		Vector3 tempPos = transform.position;
		tempPos.z += velocity.x;
		tempPos.y += velocity.y;
		transform.position = tempPos;
	}

}
