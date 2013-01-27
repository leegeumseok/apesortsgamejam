using UnityEngine;
using System.Collections;

public class ShrinkEnemyAnim : MonoBehaviour 
{
    public float ShrinkSpeed = 10.0f;
    public float ExpandSpeed = 60.0f;

    void Start()
    {
        this.animation["Shrink"].speed = ShrinkSpeed;
        this.animation["Expand"].speed = ExpandSpeed;
        this.animation.Play("Expand");
    }

    void Shrink()
    {
        this.animation.Play("Shrink");
    }

    void Expand()
    {
        this.animation.Play("Expand");
    }
}
