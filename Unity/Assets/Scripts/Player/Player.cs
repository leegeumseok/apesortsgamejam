using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    private static Player _instance = null;
    public static Player Instance
    {
        get
        {
            return _instance;
        }

        private set
        {
            if (_instance != null)
                Debug.LogError("Multiple Player singletons");
            _instance = value;
        }
    }
	
	//Player Components
	private Rigidbody mRigidbody;
	private Transform mTransform;
	private GameObject mGameObject;
<<<<<<< .mine
    public GameObject playerRenderer;
	
	//Health Variables
	public Transform[] mSpawnLocations;
	public GameObject mHeartPulse;
	public int mCurrentHealth;
	private int mMaxHealth;
	private int mSpawnSeconds = 5;
	private float mDeathTimer;
=======
    public GameObject playerRenderer;
	
	//Health Variables
	public Transform[] mSpawnLocations;
	public GameObject mHeartPulse;
	public int mCurrentHealth;
	private int mMaxHealth;
	private int mSpawnSeconds = 3;
	private float mDeathTimer;
>>>>>>> .r146
	private float mBounceBack = 5f;
<<<<<<< .mine
	private GameObject mHUD;
	
	//Pulse Attack
=======
	private GameObject mHUD;
	private GameObject mCamera;
	
	//Pulse Attack
>>>>>>> .r146
	public float mPulsePower;

<<<<<<< .mine
    public bool Alive = true;
	
	public float PulsePower { get { return mPulsePower; } set { mPulsePower = value; } }
	
	void Awake()
	{
		mRigidbody = rigidbody;
		mTransform = transform;
		mGameObject = gameObject;
		Player.Instance = this;
		mMaxHealth = mCurrentHealth;
		mHUD = GameObject.FindGameObjectWithTag("HealthUI");
	}
	
	void Update()
	{
		if(!Alive)
		{
			mDeathTimer+=Time.deltaTime;
			if(mDeathTimer > mSpawnSeconds)
			{
				Spawn();	
			}
		}
		
		if(mPulsePower >= 50f && Input.GetButtonDown("Jump"))
		{
			GameObject clone;
			clone = Instantiate(mHeartPulse, Vector3.zero, Quaternion.identity) as GameObject;
			clone.GetComponent<Pulse>().mMaxRadius = mPulsePower;
			mPulsePower = 0f;
		}
	}
	
	public void Death()
	{
		mDeathTimer = 0;
=======
    public bool Alive = true;
	
	public float PulsePower { get { return mPulsePower; } set { mPulsePower = value; } }
	
	void Awake()
	{
		mRigidbody = rigidbody;
		mTransform = transform;
		mGameObject = gameObject;
		Player.Instance = this;
		mMaxHealth = mCurrentHealth;
		mCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	void Update()
	{
		if(!Alive)
		{
			mDeathTimer+=Time.deltaTime;
			if(mDeathTimer > mSpawnSeconds)
			{
				Spawn();	
			}
		}
		
		if(mPulsePower >= 50f && Input.GetButtonDown("Jump"))
		{
			GameObject clone;
			clone = Instantiate(mHeartPulse, Vector3.zero, Quaternion.identity) as GameObject;
			clone.GetComponent<Pulse>().mMaxRadius = mPulsePower;
			mPulsePower = 0f;
		}
	}
	
	public void Death()
	{
		mDeathTimer = 0;
>>>>>>> .r146
		Alive = false;
        playerRenderer.SetActive(false);
		foreach (Transform child in mTransform)
		{
			if(child.renderer!=null)
				child.renderer.enabled = false;
		}
<<<<<<< .mine
        this.Alive = false;
		mRigidbody.isKinematic = true;
	}
	
	public void Spawn()
	{
		GameObject clone;
		mRigidbody.isKinematic = false;
		
		//Find closest spawn location
		float closestDistance = Mathf.Infinity;
		Transform closestSpawn=null;
		foreach(Transform spawn in mSpawnLocations)
		{
			float distance = Vector3.Magnitude(mTransform.position-spawn.position);
			if(distance < closestDistance)
			{
				closestDistance = distance;
				closestSpawn = spawn;
			}
		}
		
		
		if(closestSpawn != null)
			mTransform.position = closestSpawn.position;
		mCurrentHealth = mMaxHealth;
=======
        this.Alive = false;
		mRigidbody.isKinematic = true;
		mCamera.GetComponent<GameStatsUI>().respawnTimerCount = 5;
	}
	
	public void Spawn()
	{
		GameObject clone;
		mRigidbody.isKinematic = false;
		
		//Find closest spawn location
		float closestDistance = Mathf.Infinity;
		Transform closestSpawn=null;
		foreach(Transform spawn in mSpawnLocations)
		{
			float distance = Vector3.Magnitude(mTransform.position-spawn.position);
			if(distance < closestDistance)
			{
				closestDistance = distance;
				closestSpawn = spawn;
			}
		}
		
		
		if(closestSpawn != null)
			mTransform.position = closestSpawn.position;
		mCurrentHealth = mMaxHealth;
>>>>>>> .r146
		mGameObject.collider.enabled = true;
        playerRenderer.SetActive(true);
		Alive = true;
		clone = Instantiate(mHeartPulse, mTransform.position, Quaternion.identity) as GameObject;		
		
	}

    /*
	void OnCollisionEnter(Collision collision)
	{
		Transform heldEnemy = mGameObject.GetComponent<PlayerController>().GrabItem;
		if(collision.gameObject.tag == "Enemy")
		{
			if(heldEnemy)
				if(heldEnemy.gameObject == collision.gameObject)
					return;
			mCurrentHealth--;
			float currentHealth = mCurrentHealth;
			float maxHealth = mMaxHealth;
			currentHealth /= mMaxHealth;
			mHUD.GetComponent<HealthUI>().SetHealth(currentHealth);
			collision.rigidbody.AddForce((Vector3.Normalize(collision.transform.position-mTransform.position))*mBounceBack, ForceMode.Impulse);
			if (mCurrentHealth <= 0)
				Death();
		}
	}
     * */

    void AssignDamage(float damage)
    {
		if(Alive)
		{
	        Debug.Log("Player just recieved " + damage + " damage!");
	        mCurrentHealth -= (int)damage;
	        if (mCurrentHealth <= 0)
	            Death();
		}
    }
}
