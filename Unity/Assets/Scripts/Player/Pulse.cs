using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {
	
	public int mSegments;
	public float mRadius;
	public float mMaxRadius;
	private float mCurrAngle = 0f;
	private LineRenderer mLine;

	// Use this for initialization
	void Awake () {
		mLine = gameObject.GetComponent<LineRenderer>();
		mLine.SetVertexCount(mSegments+1);
		mLine.useWorldSpace = false;
		CreatePoints();
	
	}
	
	
	void CreatePoints ()
	{
		float x;
		float y=0f;
		float z;
		
		float angle = 0f;
		
		for (int i = 0; i < (mSegments + 1); i++)
		{
			x = Mathf.Sin(Mathf.Deg2Rad*mCurrAngle);
			z = Mathf.Cos(Mathf.Deg2Rad*mCurrAngle);
			mLine.SetPosition(i, new Vector3(x,y,z) * mRadius);
			mCurrAngle += (360f / mSegments);
			
		}
	}
	
	// Update is called once per frame
	void Update (){
		if(mRadius < mMaxRadius)
		{
			mRadius++;
			CreatePoints();
		}
		else
			Destroy(this.gameObject);
	}
}
