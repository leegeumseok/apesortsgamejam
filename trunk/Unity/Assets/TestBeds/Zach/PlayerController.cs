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
	private GameObject mGameObject;
	
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
	public float mPunchRadius;
	public float mMaxPunchPower;
	public float mMinPunchPower;
	
	
	//Grab Variables
	private Transform mGrabItem;
	private float mGrabSpeed = 8f;
	private float mHoldingDist = .5f;
	public float mGrabRadius;
	
	//Damage Variables
	public float mGrabDamage;
	public float mPunchDamage;
	

	private Player mPlayer;
	
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
	
	//Getters and Setters
	public Transform GrabItem { get { return mGrabItem;} }
	

	// Use this for initialization
	void Awake () {
		
		mRigidbody = rigidbody;
		mTransform = transform;
		mLeftHand = mTransform.FindChild("LeftHand");
		mRightHand = mTransform.FindChild("RightHand");
		mGameObject = gameObject;
		mPlayer = GetComponent<Player>();

	}
	
	// Update is called once per frame
	void Update () {
		
		//Left Hand States
		if(mPlayer.Alive)
			{
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
					//Assign damage to gripped enemy
					//mGrabItem.gameObject.SendMessage("ApplyDamage", mGrabDamage/Time.deltaTime);
					
					//Keep item in front of you
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
	
	}
			
	void FixedUpdate() 
    {
		
		//Remove physics simulation from player controller
		mRigidbody.angularVelocity = Vector3.zero;
		
		//Get Direction Player is Moving
		mHorizInput = Input.GetAxis("Horizontal");
		mForwardInput = Input.GetAxis("Vertical");
		if(mPlayer.Alive)
        	MovePlayer();
	}
	
	/*
	 * Moves Player
	 */
	void MovePlayer() 
    {
		
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
		Collider[] colliders = Physics.OverlapSphere(punchPos, mPunchRadius);
		
		foreach (Collider hit in colliders)
		{
			Vector3 forward = mTransform.TransformDirection(Vector3.forward);
			Vector3 toOther = hit.transform.position - mTransform.position;
			if(hit.rigidbody && hit.tag != "Player" && Vector3.Dot(forward, toOther) > 0f)
			{
				
				//Knock Enemy back
				float distance = Vector3.Magnitude(hit.transform.position-mLeftHand.position);
				float punchPower = Mathf.Min(mMaxPunchPower/distance, mMinPunchPower);
				hit.rigidbody.AddForce((Vector3.Normalize(hit.transform.position-mLeftHand.position))*punchPower, ForceMode.Impulse);

                hit.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
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
			Collider[] colliders = Physics.OverlapSphere(grabPos, mGrabRadius);
			
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