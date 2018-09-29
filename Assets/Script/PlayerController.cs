using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	private Animator anim;
	private Rigidbody2D rb2d;
	public Transform posPe;
	public LayerMask Ground;
	public Transform WallCheck;
	[HideInInspector] public bool touchWall = false;
	[HideInInspector] public bool tocaChao = false;
	public float Velocidade;
	public float ForcaPulo = 1000f;
	[HideInInspector] public bool viradoDireita = true, Jumping;
	public bool jump;
	public BoxCollider2D bc;
	public BoxCollider2D slidecol;
	public float slideSpeed = 5f;
	public bool isSlide = false;
	public bool SideCheck;
	public WaitForSeconds slidetime;

	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		bc = bc.GetComponent<BoxCollider2D> ();
		slidecol.enabled = false;
	}
	void Update () {
		//The groundcheck
		tocaChao = Physics2D.Linecast (transform.position, posPe.position, 1 << LayerMask.NameToLayer ("Ground"));
		touchWall = Physics2D.Linecast (transform.position, WallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		Fall ();
		if (Input.GetKeyDown("space")) {
			bc.size = new Vector3 (0.5509329f, 0.820936f, 0);
			bc.offset = new Vector3 (0.1267829f, 0.820936f, 0);
			Jump ();
		}
			
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			Doslide ();
		}
	}
	void FixedUpdate()
	{
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
			Debug.Log ("tocachao");
			anim.SetBool ("Fall", false);
			anim.SetBool ("Wall Slide", false);
		}
	}
	void Jump(){
		if (tocaChao && rb2d.velocity.y >= 0 ) {
			rb2d.AddForce (new Vector2 (0f, ForcaPulo));
			anim.SetTrigger ("Jump");
			anim.SetBool ("Wall Slide", false);
			Jumping = true;
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
	 void  OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.CompareTag ("Obstacle")) {
			if (tocaChao) {
				anim.SetTrigger ("Hurt");
			} else {
				anim.SetTrigger ("Fall Hurt");
			}
			jump = false;
			anim.SetTrigger ("Stand Hand");
			Update ();
		}
		}
}

	//Método de dano do player
	/*public void SubtraiVida()
	
		vida.fillAmount-=0.1f;
		if (vida.fillAmount <= 0) {
			MC.GameOver();
			Destroy(gameObject);
		}*/
