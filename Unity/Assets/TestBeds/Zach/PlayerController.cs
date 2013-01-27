using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	#region Member Variables
	
	public bool DEBUG = false;
	
	/*
	 * Player Variables
	 */
	//Spawning
	public Transform mSpawnLocation;
	
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
	public float mPunchPower;
	
	//Grab Variables
	private Transform mGrabItem;
	private float mGrabSpeed = 8f;
	private float mHoldingDist = .5f;
	public float mGrabRadius;
	
	//Damage Variables
	public float mGrabDamage;
	public float mPunchDamage;
	
	//Health Variables
	public int mCurrentHealth;
	private int mMaxHealth;
	private int mSpawnSeconds = 5;
	private bool isDead = false;
	private float mDeathTimer;
	
	
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
	public bool IsDead { get { return isDead; } }

	// Use this for initialization
	void Awake () {
		
		mRigidbody = rigidbody;
		mTransform = transform;
		mLeftHand = mTransform.FindChild("LeftHand");
		mRightHand = mTransform.FindChild("RightHand");
		mMaxHealth = mCurrentHealth;
		mGameObject = gameObject;
		
		if(mSpawnLocation == null)
		{
			Debug.Log("You probably need to assign a spawn location");
		}
		

	
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log(mCurrentHealth);
		
		if(!isDead)
		{
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
			
	void FixedUpdate() {
		
		//Remove physics simulation from player controller
		mRigidbody.angularVelocity = Vector3.zero;
		
		//Get Direction Player is Moving
		mHorizInput = Input.GetAxis("Horizontal");
		mForwardInput = Input.GetAxis("Vertical");
		
		if(!isDead)
			MovePlayer();
		else
		{
			mDeathTimer += Time.deltaTime;
			if (mDeathTimer > mSpawnSeconds)
				Spawn();
		}
		
		
		
		Debug.Log(mDeathTimer);
		
			
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
		Collider[] colliders = Physics.OverlapSphere(punchPos, mPunchRadius);
		
		foreach (Collider hit in colliders)
		{
			Vector3 forward = mTransform.TransformDirection(Vector3.forward);
			Vector3 toOther = hit.transform.position - mTransform.position;
			if(hit.rigidbody && hit.tag != "Player" && Vector3.Dot(forward, toOther) > 0f)
			{
				
				//Knock Enemy back
				float distance = Vector3.Magnitude(hit.transform.position-mLeftHand.position);
				hit.rigidbody.AddForce((Vector3.Normalize(hit.transform.position-mLeftHand.position))*(mPunchPower/distance), ForceMode.Impulse);
				
				//Apply damage to enemy
				//hit.gameObject.SendMessage("ApplyDamage", mPunchDamage/distance);
				
				//hit.rigidbody.AddExplosionForce(punchPower,punchPos,punchRadius, 3.0f);
				/*
				Vector3 closestPoint = hit.ClosestPointOnBounds(mRightHand.position);
				Vector3 direction = closestPoint - mRightHand.position;
				direction.Normalize();
				direction *= punchPower;
				hit.rigidbody.AddForce(direction, ForceMode.Impulse);
				*/
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
	
	public void Death()
	{
		Debug.Log("Death");
		mCurrentHealth = mMaxHealth;
		mDeathTimer = 0;
		mGameObject.renderer.enabled = false;
		foreach (Transform child in mTransform)
			child.renderer.enabled = false;
		//GetComponentInChildren<Renderer>().enabled = false;
		isDead = true;
	}
	
	public void Spawn()
	{
		mTransform.position = mSpawnLocation.position;
		mGameObject.renderer.enabled = true;
		GetComponentInChildren<Renderer>().enabled = true;

		isDead = false;
	}
	
	
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Enemy")
		{
			mCurrentHealth--;
			if (mCurrentHealth <= 0)
				Death();
		}
	}
}