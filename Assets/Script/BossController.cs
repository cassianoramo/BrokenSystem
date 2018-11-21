using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController {

	public BoxCollider2D bcAttack;
	public GameObject BossAttack;

	void Start () {
		bcAttack = GetComponent<BoxCollider2D> ();
		health = 10;
		BossAttack.SetActive (false);
		PlayerRay = 2.5f;
	}

	void Update () {
		if (isAlive == false) {
			return;
		}
		float distance = PlayerPosition ();
		isMoving = (distance <= AttackRay);
		FrontPlayer = (distance <= PlayerRay); 
		if (isMoving) {
			if ((player.position.x > transform.position.x && !right) || (player.position.x < transform.position.x && right)) {
				Flip1 ();
			}
		}
		if (canHurt) {
			hurt = false;
		}
		timeAttack -= Time.deltaTime;
		if (timeAttack >= 0) {
			hurt = true;
		}
		if (FrontPlayer && timeAttack <= 0) {
			if (hurt) {
				return;
			}
			AttackingBoss = true;
			anim.SetTrigger ("AttackBoss1");
			BossAttack.SetActive (true);
			//AttackEnemy ();
			canHurt = false;
		}
	}
	void FixedUpdate(){
		
		if (isMoving) {
			if (hurt || AttackingBoss) {
				return;
			}
			rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
			anim.SetTrigger ("WalkBoss");
		} else {
			anim.SetTrigger ("StandBoss");
			AttackRay = 20;
		}
		if (health <= 0) {
			anim.SetTrigger ("DeadBoss");
			isAlive = false;
			StartCoroutine ("Deadbc");
		} else
		bcEnemy.size = new Vector3 (4.573245f, 6.431195f, 0);
		bcEnemy.offset = new Vector3 (-0.832418f, -0.05688047f, 0);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Attack")&& canHurt) {
			anim.SetTrigger ("HurtBoss");
			health -= 1;
			AttackRay = 0;
			hurt = true;
			canHurt = false;
			Debug.Log ("Enemy Hurt");
			StartCoroutine ("stopHurt");
		}
	}
	IEnumerator stopHurt (){
		yield return new WaitForSeconds(1.2f);
		Debug.Log ("stopattack");
		hurt = false;
		canHurt = true;
	}
	IEnumerator Deadbc (){
		yield return new WaitForSeconds(0.5f);
		bcEnemy.size = new Vector3 (5.077364f, 0.3983829f, 0);
		bcEnemy.offset = new Vector3 (-0.5803585f,-3.667428f, 0);
	}
	public void Flip1(){
		speed *= -1;
		right = !right;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}
	public void AttackBoss(){
		//BossAttack.SetActive (true);
		bcAttack.size = new Vector3 (10.70534f, 2.510119f, 0);
		bcAttack.offset = new Vector3 (0.4229562f, -1.104609f, 0);
		Debug.Log ("AttackEnemy");
		StartCoroutine ("stopAttack");
		timeAttack = 1.5f;
	}
	IEnumerator stopAttack(){
		yield return new WaitForSeconds(0.1f);
		Debug.Log ("StopAttack");
		BossAttack.SetActive (false);
		AttackingBoss = false;
		canHurt = true;
		hurt = false;
	}
}

