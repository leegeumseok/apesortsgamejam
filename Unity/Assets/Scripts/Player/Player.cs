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

    public bool Alive = true;
	

	
	public void Death()
	{
		Debug.Log("Death");
		mCurrentHealth = mMaxHealth;
		mDeathTimer = 0;
		mGameObject.renderer.enabled = false;
		foreach (Transform child in mTransform)
			child.renderer.enabled = false;
	}
	
	public void Spawn()
	{
		mTransform.position = mSpawnLocation.position;
		mGameObject.renderer.enabled = true;
		GetComponentInChildren<Renderer>().enabled = true;
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
