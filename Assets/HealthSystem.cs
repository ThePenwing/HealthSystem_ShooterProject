using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    // Variables

    public int health;
    public string healthStatus;
    public int shield;
    public int lives;

    // Optional XP system variables
    public int xp;
    public int level;

    public HealthSystem()
    {
        ResetGame();
    }


    public string ShowHUD()
    {
        // Implement HUD display
        return "Health: " + health + " | " + "Shield: " + shield + " | " + "Lives: " + lives +
            "\x0A" + "Level: " + level + " | " + "Xp: " + xp +
            "\x0A" + healthStatus;

    }

    (int, int) ClampAndGetRemainder(int value, int min, int max)
    {
        int clampedValue = Mathf.Clamp(value, min, max);
        int remainder = value - clampedValue;
        return (clampedValue, remainder);
    }

    public void TakeDamage(int damage)
    {
        // Implement damage logic
        if (damage > 0)
        {
            var (shieldClampedValue, shieldRemainder) = ClampAndGetRemainder(shield - damage, 0, 100); //Shield
            shield = shieldClampedValue;

            if (shieldRemainder < 0)
            {
                var (healthClampedValue, healthRemainder) = ClampAndGetRemainder(health + shieldRemainder, 0, 100); //Health
                health = healthClampedValue;
                if (health <= 0)
                {
                    Revive();
                }

                if (health <= 10)
                {
                    healthStatus = "Imminent Danger";
                }
                else if (health <= 50)
                {
                    healthStatus = "Badly Hurt";
                }
                else if (health <= 75)
                {
                    healthStatus = "Hurt";
                }
                else if (health <= 90)
                {
                    healthStatus = "Healthy";
                }
                else if (health <= 100)
                {
                    healthStatus = "Perfect Health";
                }
            }
        }
    }

    public void Heal(int hp)
    {
        // Implement healing logic
        if (hp > 0)
        {
            health = Mathf.Clamp(health + hp, 0, 100);
        }
    }

    public void RegenerateShield(int hp)
    {
        // Implement shield regeneration logic
        if (hp > 0)
        {
            shield = Mathf.Clamp(shield + hp, 0, 100);
        }
    }

    public void Revive()
    {
        // Implement revive logic
        if (lives > 0)
        {
            health = 100;
            healthStatus = "Perfect Health";
            shield = 100;
            lives = lives - 1;
        }
        else
        {
            //ResetGame(); keeping this on will make enemies unkillable cause all their stats reset on death
        }
    }

    public void ResetGame()
    {
        // Reset all variables to default values
        health = 100;
        healthStatus = "Perfect Health";
        shield = 100;
        lives = 3;

        xp = 0;
        level = 1;
    }

    // Optional XP system methods
    public void IncreaseXP(int exp)
    {
        // Implement XP increase and level-up logic
        xp += exp;
        level += xp / 100;
        xp = xp % 100;
    }
}