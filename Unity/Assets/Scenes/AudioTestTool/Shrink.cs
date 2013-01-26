using UnityEngine;
using System.Collections;

public class Shrink : BeatReceiver 
{
    public float scale = 1.0f;

    void Start()
    {
        SongPlayer.Instance.RegisterReceiver(this);
    }

	// Update is called once per frame
	void Update () 
    {
        if (scale > 1.0f)
            scale *= 0.93f;
        if (scale < 1.0f)
            scale = 1.0f;

        transform.localScale = new Vector3(scale, scale, scale);
	}

    public override void OnBeat()
    {
        this.scale = 2.0f;
    }
}
