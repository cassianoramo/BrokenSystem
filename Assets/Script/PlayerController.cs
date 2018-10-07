using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	private Animator anim;
	private Rigidbody2D rb2d;
	public Transform posPe, WallCheck;
	public LayerMask Ground;
	[HideInInspector] public bool touchWall = false, tocaChao = false, viradoDireita = true;
	public float ForcaPulo = 1000f,  Velocidade, slideSpeed = 5f, TimeMaxCombo = 0.1f, TimeCombo = 0f;
	public bool jump,  SideCheck, isSlide = false, isAlive = true;
	public BoxCollider2D bc, slidecol;
	public WaitForSeconds slidetime;
	public int AnimaCombo = 0, Health;

	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		bc = bc.GetComponent<BoxCollider2D> ();
		slidecol.enabled = false;
		TimeMaxCombo=0.1f;
	}
	void Update () {
		//The groundcheck
		if(isAlive == false){
			return;
		}
		tocaChao = Physics2D.Linecast (transform.position, posPe.position, 1 << LayerMask.NameToLayer ("Ground"));
		touchWall = Physics2D.Linecast (transform.position, WallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		Fall ();
		if (Input.GetKeyDown("space")) {
			bc.size = new Vector3 (0.4929347f, 0.8221698f, 0);
			bc.offset = new Vector3 (0.1226404f, 0.01533556f, 0);
			Jump ();
		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			Doslide ();
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			AttackSword ();
		}
		if (Input.GetKeyDown (KeyCode.J)) {
			AttackHand ();
		}
		if (TimeCombo > TimeMaxCombo) {
			AnimaCombo = 0;
			TimeCombo = 0f;
		}
	}

	void FixedUpdate()
	{
		if(isAlive == false){
			return;
		}
		if( touchWall ) {
			anim.SetBool ("Wall Slide", true);
			anim.SetBool ("Fall", false);
		}else if(rb2d.velocity.y >= 0){
			anim.SetBool ("Wall Slide", false);
			anim.SetTrigger("Jump");
		}
		LayerControl ();
		//Player moviment
		float translationY = 0;
		float translationX = Input.GetAxis ("Horizontal") * Velocidade;
		transform.Translate (translationX, translationY, 0);
		transform.Rotate (0, 0, 0);
		//Animations
		if (translationX != 0 && tocaChao ) {
			anim.SetTrigger ("Run");
		   bc.size = new Vector3 (0.7746387f, 1.127496f, 0);
			bc.offset = new Vector3 (0.1723526f, -0.1373274f, 0);
		} else {
			anim.SetTrigger ("Stand Hand");
			bc.size = new Vector3 (0.7540007f,1.168771f, 0);
			bc.offset = new Vector3 (-0.07013941f, -0.1166899f, 0);
		}
		//Player direction
		if (translationX > 0 && !viradoDireita || translationX < 0 && viradoDireita) {
			if (touchWall && !tocaChao) {
				rb2d.velocity = (new Vector2 (rb2d.velocity.x, 0f));
				rb2d.AddForce (new Vector2 (0f, ForcaPulo));
			}
			Flip (); 
		}
}
	void Fall()
	{
		if (!tocaChao && rb2d.velocity.y <= 0 && !touchWall) {   
			if (touchWall) {
				anim.SetBool ("Wall Slide", true);
				anim.SetBool ("Fall", false);
			}else
			anim.SetBool ("Fall", true);
			anim.ResetTrigger ("Jump");
			anim.SetBool ("Wall Slide", false);
			bc.size = new Vector3 (0.4929347f, 0.9783585f, 0);
			bc.offset = new Vector3 (0.1226404f, -0.0627588f, 0);
			Debug.Log ("Fall");
		}	
		if (tocaChao) {
			anim.SetBool ("Fall", false);
			anim.SetBool ("Wall Slide", false);
		}
	}
	void Jump(){
		if (tocaChao && rb2d.velocity.y >= 0 ) {
			rb2d.AddForce (new Vector2 (0f, ForcaPulo));
			anim.SetTrigger ("Jump");
			anim.SetBool ("Wall Slide", false);

		}
}
	//Flip script
	void Flip()
	{
		viradoDireita = !viradoDireita;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}
	void LayerControl(){
		if (!tocaChao) {
			anim.SetLayerWeight (1, 1);
		} else {
			anim.SetLayerWeight (1, 0);
		}
	}
	private void Doslide(){
		isSlide = true;
		anim.SetTrigger ("isSliding");
		slidecol.enabled = true;
		bc.enabled = false;
		StartCoroutine ("stopSlide");
	}
		IEnumerator stopSlide(){
		yield return new WaitForSeconds(1f);
		anim.SetTrigger ("Stand Hand");
		slidecol.enabled = false;
		bc.enabled = true;
		isSlide = false;
		}
	void AttackHand(){
		TimeCombo = +Time.deltaTime;
		if (tocaChao && AnimaCombo == 0) {
			anim.SetTrigger ("Punch1");
			//AnimaCombo = 1;
		}
		if (tocaChao && AnimaCombo == 1) {
			anim.SetTrigger ("Punch2");
			AnimaCombo = 2;
		}
		Debug.Log (TimeCombo);
		}
	void AttackSword (){
		if (tocaChao && AnimaCombo == 0) {
			anim.SetTrigger ("Sword1");
		//	AnimaCombo = 1;
		}
	}
	 void  OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.CompareTag ("Obstacle")) {
			if (tocaChao) {
				anim.SetTrigger ("Hurt");
				Health--;
			} else {
				anim.SetTrigger ("Fall Hurt");
				Health--;
			}
			if (Health < 1) {
				anim.SetTrigger("Dead");
				isAlive = false;
			}
			jump = false;
			anim.SetTrigger ("Stand Hand");
			Update ();
		}
	}
}
