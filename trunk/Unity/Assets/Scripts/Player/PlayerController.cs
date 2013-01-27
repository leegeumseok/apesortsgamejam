using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	#region Member Variables
	
	public bool DEBUG = false;
	
	/*
	 * Player Variables
	 */
	
	//Speed
	public float mWallResistance = 20f;
	public float mAreaRadius;
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

    public PlayerPunchAnim PlayerPunch = null;
    public ParticleSystem PunchParticle = null;
    protected bool LeftHand = false;
    
	
	//Determines physics placed in punch
	public float mPunchRadius;
	public float mMaxPunchPower;
	public float mMinPunchPower;
	public float mPunchRecharge;
	private float mPunchTimer;
	
	
	//Grab Variables
	private Transform mGrabItem;
	private float mGrabSpeed = 8f;
	private float mHoldingDist = .5f;
	public float mGrabRadius;
	public float mGrabRecharge;
	private float mGrabTimer;
	
	//Damage Variables
	public int mGrabDamage;
	public int mPunchDamage;
	

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
				mPunchTimer += Time.deltaTime;
				if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown("j")) && mPunchTimer > mPunchRecharge)
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
				mGrabTimer += Time.deltaTime;
				if ((Input.GetButtonDown("Fire2")|| Input.GetKeyDown("k")) && mGrabTimer > mGrabRecharge)
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
		
		//Get Direction Player is Moving
		mHorizInput = Input.GetAxis("Horizontal");
		mForwardInput = Input.GetAxis("Vertical");
		if(mPlayer.Alive)
		{
			//Remove physics simulation from player controller
			mRigidbody.angularVelocity = Vector3.zero;
        	MovePlayer();
		}
	}
	
	/*
	 * Moves Player
	 */
	void MovePlayer() 
    {
		//Return player to center if they've left the radius of circle
		float distance = Vector3.Magnitude(mTransform.position - Vector3.zero);
		
		if(distance > mAreaRadius)
		{
			Vector3 forceDirection = Vector3.Normalize(Vector3.zero - mTransform.position)*mWallResistance;
			mRigidbody.AddForce(forceDirection, ForceMode.VelocityChange);
		}
		
		
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
		//mLeftHand.renderer.enabled = true;
		mLeftState = MState.Attacking;
        PunchParticle.Play();

        if (LeftHand == true)
            PlayerPunch.LeftPunch();
        else
            PlayerPunch.RightPunch();
        LeftHand = !LeftHand;
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

                hit.gameObject.SendMessage("OnHit");
                hit.gameObject.SendMessage("OnDamaged", mPunchDamage);
			}
		}
	}
	
	void AttackExit() {
		mLeftHand.renderer.enabled = false;	
		mLeftState = MState.Idle;
		mPunchTimer = 0;
	}
	
	void GrabEnter() {
		//mRightHand.renderer.enabled = true;
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
				if(hit.rigidbody && hit.tag == "Enemy" && Vector3.Dot(forward, toOther) > 0)
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

            if (closestEnemy != null)
            {
                mGrabItem = closestEnemy.transform;
                mGrabItem.SendMessage("OnGrabbed");
            }
		}
	}

    public void OnGrabbedDeath()
    {
        if (mRightState == MState.Attacking)
        {
            this.mGrabItem = null;
            this.GrabExit();
        }
    }

	void GrabExit() {
        if (this.mGrabItem != null)
        {
            mGrabItem.SendMessage("OnReleased");
        }
		//mRightHand.renderer.enabled = false;
		mRightState = MState.Idle;
		mGrabItem = null;
		mGrabTimer = 0;
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