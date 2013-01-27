using UnityEngine;
using System.Collections;

public class PlayerPunchAnim : MonoBehaviour 
{
    public float RightPunchSpeed = 1.0f;
    public float LeftPunchSpeed = 1.0f;

    public void RightPunch()
    {
        animation.Play("right punch");
    }

    public void LeftPunch()
    {
        animation.Play("left punch");
    }

	void Start() 
    {
        animation["right punch"].speed = RightPunchSpeed;
        animation["left punch"].speed = LeftPunchSpeed;
	}
}
