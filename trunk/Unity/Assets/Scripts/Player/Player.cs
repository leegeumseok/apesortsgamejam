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
	
	//Health Variables
	public Transform mSpawnLocation;
	public int mCurrentHealth;
	private int mMaxHealth;
	private int mSpawnSeconds = 5;
	private float mDeathTimer;
	private float mBounceBack = 5f;

    public bool Alive = true;
	
	void Awake()
	{
		mRigidbody = rigidbody;
		mTransform = transform;
		mGameObject = gameObject;
		Player.Instance = this;
		mMaxHealth = mCurrentHealth;
		if(mSpawnLocation == null)
		{
			Debug.Log("You probably need to assign a spawn location");
		}
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
	}
	
	public void Death()
	{
		mDeathTimer = 0;
		Alive = false;
		mGameObject.renderer.enabled = false;
		mGameObject.collider.enabled = false;
		foreach (Transform child in mTransform)
		{
			if(child.renderer!=null)
				child.renderer.enabled = false;
		}
        this.Alive = false;
	}
	
	public void Spawn()
	{
		mTransform.position = mSpawnLocation.position;
		mCurrentHealth = mMaxHealth;
		mGameObject.collider.enabled = true;
		mGameObject.renderer.enabled = true;
		mTransform.FindChild("Front").renderer.enabled = true;
		Alive = true;
		
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Transform heldEnemy = mGameObject.GetComponent<PlayerController>().GrabItem;
		if(collision.gameObject.tag == "Enemy")
		{
			if(heldEnemy)
				if(heldEnemy.gameObject == collision.gameObject)
					return;
			mCurrentHealth--;
			collision.rigidbody.AddForce((Vector3.Normalize(collision.transform.position-mTransform.position))*mBounceBack, ForceMode.Impulse);
			if (mCurrentHealth <= 0)
				Death();
		}
	}
}
