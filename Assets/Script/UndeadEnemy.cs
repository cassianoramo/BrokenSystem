using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadEnemy : EnemyController {

	void Start () {
		health = 3;
	}
	void Update () {
		float distance = PlayerPosition ();
		isMoving = (distance <= AttackRay);
		if (isMoving) {
			if ((player.position.x > transform.position.x && right) || (player.position.x < transform.position.x && !right)){
				Flip ();
			}
		}
	}
	void FixedUpdate(){
		if (isMoving) {
			rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
			anim.SetTrigger ("EnemyWalk");
		} else {
			anim.SetTrigger ("EnemyIdle");
		}
	}
}
