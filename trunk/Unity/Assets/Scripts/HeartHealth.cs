using UnityEngine;
using System.Collections;

public class HeartHealth : MonoBehaviour
{
    public enum GameOutcome
    {
        Won,
        Lost
    }

    private GameStatsUI overlay;
    private float nextRegeneratedHitPoint;
    public int maxHealth = 500;
    public int currentHealth = -1;
    public float regenerationPerSecond = 0;
    public GameObject bloodEffect;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        nextRegeneratedHitPoint = 1 / regenerationPerSecond; // returns +infinity when regenerationPerSecond = 0
        var overlays = GameObject.FindGameObjectsWithTag("MainCamera");
        if (overlays.Length > 0)
        {
            overlay = overlays[0].GetComponent<GameStatsUI>();
            if (overlay != null)
            {
                overlay.heartHealth = currentHealth;
                overlay.heartHealthMax = maxHealth;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        nextRegeneratedHitPoint -= Time.deltaTime;
        if (nextRegeneratedHitPoint <= 0 && currentHealth < maxHealth)
        {
            currentHealth++;
            nextRegeneratedHitPoint = 1 / regenerationPerSecond;
        }
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;

        if (bloodEffect != null)
        {
            for (int i = 0; i < damage; i++)
                Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EndGame(GameOutcome.Lost);
        }

        if (overlay != null)
        {
            overlay.heartHealth = currentHealth;
        }
    }

    private void EndGame(GameOutcome outcome)
    {
        // TODO stuff here!
        Debug.LogError("YOUR HEART WAS BLOWN TO PIECES </3");
    }
}
