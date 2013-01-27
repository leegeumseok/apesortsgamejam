using UnityEngine;
using System.Collections;

public class HeartHealth : MonoBehaviour
{
    public enum GameOutcome
    {
        Won,
        Lost
    }

    private float nextRegeneratedHitPoint;
    public int maxHealth = 500;
    public int currentHealth = -1;
    public float regenerationPerSecond = 0;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        nextRegeneratedHitPoint = 1 / regenerationPerSecond; // returns +infinity when regenerationPerSecond = 0
    }

    // Update is called once per frame
    void Update()
    {
        nextRegeneratedHitPoint -= Time.deltaTime;
        if (nextRegeneratedHitPoint <= 0 && currentHealth < maxHealth)
        {
            currentHealth++;
            nextRegeneratedHitPoint += 1 / regenerationPerSecond;
        }
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EndGame(GameOutcome.Lost);
        }

        // TODO animate blood with particles or something
    }

    private void EndGame(GameOutcome outcome)
    {
        // TODO stuff here!
        Debug.LogError("YOUR HEART WAS BLOWN TO PIECES </3");
    }
}
