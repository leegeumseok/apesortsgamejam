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
	
	private Transform mLeftHand;
	private Transform mRightHand;
	
	//Determines physics placed in punch
	public float punchRadius;
	public float punchPower;
	
	//Grab Variables
	private Transform mGrabItem;
	private float mGrabSpeed = 8f;
	private float mHoldingDist = .5f;
	public float grabRadius;
	
	
	#endregion
	
	/*
	 * Different Player States
	 */
	private enum MState
	{
		Idle, Attacking,
	}
	
	//Each hand has a different state
	private MState mLeftState = MState.Idle;
	private MState mRightState = MState.Idle;

	// Use this for initialization
	void Awake () {
		
		mRigidbody = rigidbody;
		mTransform = transform;
		mLeftHand = mTransform.FindChild("LeftHand");
		mRightHand = mTransform.FindChild("RightHand");

	
	}
	
	// Update is called once per frame
	void Update () {
		
		//Left Hand States
		switch(mLeftState)
		{
		case MState.Idle:
			if (Input.GetButtonDown("Fire1") || Input.GetKeyDown("j"))
			{
				AttackEnter();	
			}
			
			break;
		case MState.Attacking:
			if (Input.GetButtonUp("Fire1")|| Input.GetKeyUp("j"))
			{
				AttackExit();	
			}

			break;
		default:
			break;
		}
		
		//Right Hand States
		switch(mRightState)
		{
		case MState.Idle:
			if (Input.GetButtonDown("Fire2")|| Input.GetKeyDown("k"))
			{
				GrabEnter();	
			}
			
			break;
		case MState.Attacking:
			if (Input.GetButtonUp("Fire2") || Input.GetKeyUp("k"))
			{
				GrabExit();	
			}
			
			if (mGrabItem != null)
			{
				float distance = CalculateDistance(mGrabItem.position, mRightHand);
				if (distance > 1f)
					mGrabItem.position = Vector3.Lerp(mGrabItem.position,
						mRightHand.position + mRightHand.forward*mHoldingDist, Time.deltaTime * mGrabSpeed);
				
				mGrabItem.LookAt(mTransform);
			}
			

			break;
		default:
			break;
		}
	
	}
			
	void FixedUpdate() {
		
		//Remove physics simulation from player controller
		mRigidbody.angularVelocity = Vector3.zero;
		
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
		mLeftHand.renderer.enabled = true;
		mLeftState = MState.Attacking;
		//mLeftHand.collider.enabled = true;
		
		Vector3 punchPos = mLeftHand.position;
		Collider[] colliders = Physics.OverlapSphere(punchPos, punchRadius);
		
		foreach (Collider hit in colliders)
		{
			Vector3 forward = mTransform.TransformDirection(Vector3.forward);
			Vector3 toOther = hit.transform.position - mTransform.position;
			if(hit.rigidbody && hit.tag != "Player" && Vector3.Dot(forward, toOther) > 0f)
			{
				hit.rigidbody.AddExplosionForce(punchPower,punchPos,punchRadius, 3.0f);
			}
		}
	}
	
	void AttackExit() {
		mLeftHand.renderer.enabled = false;	
		mLeftState = MState.Idle;
		//mLeftHand.collider.enabled = false;
	}
	
	void GrabEnter() {
		mRightHand.renderer.enabled = true;
		mRightState = MState.Attacking;
		
		if (mGrabItem == null)
		{
			float closestEnemyDist = Mathf.Infinity;
			Vector3 grabPos = mRightHand.position;
			Collider closestEnemy = null;
			Collider[] colliders = Physics.OverlapSphere(grabPos, grabRadius);
			
			foreach (Collider hit in colliders)
			{
				Vector3 forward = mTransform.TransformDirection(Vector3.forward);
				Vector3 toOther = hit.transform.position - mTransform.position;
				if(hit.rigidbody && hit.tag != "Player" && Vector3.Dot(forward, toOther) > 0)
				{
					Vector3 closestPoint = hit.ClosestPointOnBounds(mRightHand.position);
					float tempDistance = CalculateDistance(closestPoint, mRightHand);
					if (tempDistance < closestEnemyDist)
					{
						closestEnemyDist = tempDistance;
						closestEnemy = hit;
					}
				}
			}
			
			if(closestEnemy != null)
				mGrabItem = closestEnemy.transform;
		}
		
		//mRightHand.collider.enabled = true;
	}
	
	void GrabExit() {
		mRightHand.renderer.enabled = false;
		mRightState = MState.Idle;
		mGrabItem = null;
		//mRightHand.collider.enabled = false;
	}
	
	#endregion
	
	#region Utility Functions
	private float CalculateDistance (Vector3 target, Transform point)
	{
		Vector3 toTarget = target - point.position;
		float distance = toTarget.magnitude;
		return distance;
	}
	#endregion
}
