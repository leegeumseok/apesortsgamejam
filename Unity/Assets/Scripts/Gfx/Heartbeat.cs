using UnityEngine;
using System.Collections;

public class Heartbeat : BeatReceiver
{
    public float currentScale = 16.0f;
    public float minScale = 16.0f;
    public float maxScale = 22.0f;

    void Start()
    {
        BeatManager.Instance.RegisterReceiver(this, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScale > minScale)
            currentScale *= 0.97f;
        if (currentScale < minScale)
            currentScale = minScale;

        transform.localScale = new Vector3(
            currentScale, currentScale, currentScale);
    }

    public override void OnBeat()
    {
        this.currentScale = maxScale;
    }
}
