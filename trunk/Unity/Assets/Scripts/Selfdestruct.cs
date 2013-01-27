using UnityEngine;
using System.Collections;

public class Selfdestruct : MonoBehaviour
{
    public float destroyAfter;

    void Start()
    {
        GameObject.Destroy(gameObject, destroyAfter);
    }
}
