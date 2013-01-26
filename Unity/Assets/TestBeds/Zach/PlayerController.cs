using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	#region Member Variables
	
	public bool DEBUG = false;
	
	/*
	 * Player Variables
	 */
	
	//Speed
	public float mAcceleration;
	public float mMaxSpeed;
	public float mRotSpeed;
	
	//Player Components
	private Rigidbody mRigidbody;
	private Transform mTransform;
	
	//Direction to move the player
	private float mHorizInput;
	private float mForwardInput;
	private Vector3 mMovementZ;
	
	/*
	 * Combat Variables
	 */
	
	private Transform mAttackZone;
	private float mAttackDamage;
	
	
	#endregion
	
	/*
	 * Different Player States
	 */
	private enum MState
	{
		Idle, Attacking
	}
	
	private MState mState = MState.Idle;
	

	// Use this for initialization
	void Awake () {
		
		mRigidbody = rigidbody;
		mTransform = transform;
		mAttackZone = mTransform.FindChild("AttackZone");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
			
	void FixedUpdate() {
		
		//Get Direction Player is Moving
		mHorizInput = Input.GetAxis("Horizontal");
		mForwardInput = Input.GetAxis("Vertical");
		
		MovePlayer();
			
	}
	
	/*
	 * Moves Player
	 */
	void MovePlayer() {
		
		//Movement Vector in Z direction
		mMovementZ = mTransform.forward * mForwardInput;
		
		//Apply force to move player
		mMovementZ *= mMaxSpeed;
		
		Vector3 velocity = mRigidbody.velocity;
		Vector3 velocityChange = mMovementZ-velocity;
		velocityChange.z = Mathf.Clamp(velocityChange.z, -mAcceleration, mAcceleration);
		velocityChange.y = 0;
		mRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
		
		//Rotate player using X input
		mTransform.Rotate(0, mHorizInput * mRotSpeed, 0);
		
		
	}
	
	
	#region State Functions
	void AttackEnter() {
		mAttackZone.renderer.enabled = true;
	}
	
	void AttackExit() {
		mAttackZone.renderer.enabled = false;	
	}
	
	#endregion
}
