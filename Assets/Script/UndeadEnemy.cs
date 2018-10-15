using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadEnemy : EnemyController {

	void Start () {
		health = 3;
		EnemyAttack.SetActive (false);
	}
	void Update () {
		float distance = PlayerPosition ();
		isMoving = (distance <= AttackRay);
		FrontPlayer = (distance <= PlayerRay); 
		if (isMoving) {
			if ((player.position.x > transform.position.x && right) || (player.position.x < transform.position.x && !right)) {
				Flip ();
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
			anim.SetTrigger ("EnemyAttack");
			canHurt = false;
		}
	}
	public void AttackEnemy(){
		EnemyAttack.SetActive (true);
		StartCoroutine ("stopAttack");
		timeAttack = 1.5f;
	}
	IEnumerator stopAttack(){
		yield return new WaitForSeconds(0.2f);
		EnemyAttack.SetActive (false);
		canHurt = true;
		hurt = false;
	}
	void FixedUpdate(){
		if (isMoving) {
			if (hurt) {
				return;
			}
			rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
			anim.SetTrigger ("EnemyWalk");
		} else {
			anim.SetTrigger ("EnemyIdle");
			AttackRay = 7;
		}
	}
	void OnTriggerEnter2D(Collider2D AttackCheck) {
		if (AttackCheck.gameObject.CompareTag ("Attack")&& canHurt) {
			anim.SetTrigger ("EnemyHurt");
			AttackRay = 0;
			hurt = true;
			canHurt = false;
			Debug.Log ("Enemy Hurt");
			StartCoroutine ("stopHurt");
		}
	}
		IEnumerator stopHurt (){
		yield return new WaitForSeconds(1.2f);
		hurt = false;
		canHurt = true;
		}
	}


