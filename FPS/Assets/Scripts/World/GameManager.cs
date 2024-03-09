using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text ammoText;

    public Text healthText;

    public int health = 100;

    public int maxHealth = 100;

    public static GameManager Instance { get; private set; }

    public int gunAmmo = 10;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        ammoText.text = gunAmmo.ToString();
        healthText.text = health.ToString();
    }

    public void LoseHealth(int healthToReduce)
    {
        health -= healthToReduce;
        CheckHealth();
    }

    public void CheckHealth()
    {
        if (health <= 0) 
        {
            Debug.Log("Moriste");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddHealth(int health)
    {
        if (this.health + health >= maxHealth)
        {
            this.health = 100;
        }

        else
        {
            this.health += health;
        }
    }
}
