using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public int health, speed;
	protected Rigidbody2D rb2d;
	public float AttackRay;
	protected Animator anim;
	protected SpriteRenderer sprite;
	protected bool isMoving = false, right = true;
	public Transform player;

	void Awake () {
		sprite = GetComponent<SpriteRenderer> ();
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		player = GameObject.Find ("Player").GetComponent<Transform> ();
	}
	protected  float PlayerPosition () {
		return Vector2.Distance(player.position, transform.position);
	}
	protected void Flip(){
		speed *= -1;
		right = !right;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}
}
