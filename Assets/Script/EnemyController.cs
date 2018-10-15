using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject EnemyAttack;
	public int health, speed;
	protected Rigidbody2D rb2d;
	public float AttackRay, PlayerRay, timeAttack;
	protected Animator anim;
	protected SpriteRenderer sprite;
	protected bool isMoving = false, right = true, hurt, canHurt,FrontPlayer = false;
	public Transform player;

	void Awake () {
		sprite = GetComponent<SpriteRenderer> ();
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		player = GameObject.Find ("Player").GetComponent<Transform> ();
		AttackRay = 7;
		PlayerRay = 1;
		canHurt = true;
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
