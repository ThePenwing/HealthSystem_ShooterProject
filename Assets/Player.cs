using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : Actor {
    public static Player instance;
    
    int lastCheckedHealth;
    int lastCheckedShield;
    int lastCheckedXp;
    int lastCheckedLevel;

    public void Awake() {
        instance = this;
        Test_TakeDamage_OnlyShield();
        Test_TakeDamage_OnlyHealth();
        Test_TakeDamage_ShieldAndHealth();
        Test_TakeDamage_NegativeDamage();
        Test_TakeDamage_LethalDamage();
        Test_TakeDamage_DepleteShieldAndHealth();

        Test_HealTests_NormalHealing();
        Test_HealTests_NegativeHeal();
        Test_HealTests_Overheal();

        Test_ShieldTests_NormalRegeneration();
        Test_ShieldTests_NegativeRegeneration();
        Test_ShieldTests_OverhealShield();

        Test_ReviveTest();

        Test_XpTest_AddXP();
    }

    public override void Die()
    {
        base.Die();
        //gameObject.SetActive(false);
    }

    public override void Update() {
        base.Update();
        if (lastCheckedHealth != healthSystem.health || lastCheckedShield != healthSystem.shield || lastCheckedLevel != healthSystem.level || lastCheckedXp != healthSystem.xp)
        {
            HealthUI.instance.textmeshpro.text = healthSystem.ShowHUD();
            lastCheckedHealth = healthSystem.health;
            lastCheckedShield = healthSystem.shield;
            lastCheckedLevel = healthSystem.level;
            lastCheckedXp = healthSystem.xp;
        }
    }

    public override Vector3 GetMovementDirection()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
    }
    public override Vector3 GetLookDirection()
    {
        return ShootDirection();
    }

    public override bool WantsToShoot()
    {
        if (healthSystem.health <= 0)
            return false;
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }

    public override Vector3 ShootDirection()
    {
        var desiredDirectionShot = (mouseReticle.instance.transform.position - transform.position);
        desiredDirectionShot.y = 0.0f;
        return desiredDirectionShot.normalized;
    }

    //Tests Below

    public void Test_TakeDamage_OnlyShield()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 100;
        system.health = 100;
        system.lives = 3;

        system.TakeDamage(10);

        Debug.Assert(90 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_TakeDamage_ShieldAndHealth()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 100;
        system.health = 100;
        system.lives = 3;

        system.TakeDamage(150);

        Debug.Assert(0 == system.shield);
        Debug.Assert(50 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_TakeDamage_OnlyHealth()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 0;
        system.health = 100;
        system.lives = 3;

        system.TakeDamage(50);

        Debug.Assert(0 == system.shield);
        Debug.Assert(50 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_TakeDamage_LethalDamage()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 0;
        system.health = 100;
        system.lives = 3;

        system.TakeDamage(100);

        Debug.Assert(100 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(2 == system.lives);
    }

    public void Test_TakeDamage_DepleteShieldAndHealth()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 100;
        system.health = 100;
        system.lives = 3;

        system.TakeDamage(200);

        Debug.Assert(100 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(2 == system.lives);
    }

    public void Test_TakeDamage_NegativeDamage()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 100;
        system.health = 100;
        system.lives = 3;

        system.TakeDamage(-10);

        Debug.Assert(100 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_HealTests_NormalHealing()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 100;
        system.health = 90;
        system.lives = 3;

        system.Heal(10);

        Debug.Assert(100 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_HealTests_Overheal()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 100;
        system.health = 100;
        system.lives = 3;

        system.Heal(10);

        Debug.Assert(100 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_HealTests_NegativeHeal()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 100;
        system.health = 75;
        system.lives = 3;

        system.Heal(-10);

        Debug.Assert(100 == system.shield);
        Debug.Assert(75 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_ShieldTests_NormalRegeneration()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 90;
        system.health = 100;
        system.lives = 3;

        system.RegenerateShield(10);

        Debug.Assert(100 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_ShieldTests_OverhealShield()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 100;
        system.health = 100;
        system.lives = 3;

        system.RegenerateShield(10);

        Debug.Assert(100 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_ShieldTests_NegativeRegeneration()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 75;
        system.health = 100;
        system.lives = 3;

        system.RegenerateShield(-10);

        Debug.Assert(75 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(3 == system.lives);
    }

    public void Test_ReviveTest()
    {
        HealthSystem system = new HealthSystem();
        system.shield = 50;
        system.health = 50;
        system.lives = 3;

        system.Revive();

        Debug.Assert(100 == system.shield);
        Debug.Assert(100 == system.health);
        Debug.Assert(2 == system.lives);
    }

    public void Test_XpTest_AddXP()
    {
        HealthSystem system = new HealthSystem();
        system.xp = 0;
        system.level = 1;

        system.IncreaseXP(650);

        Debug.Assert(50 == system.xp);
        Debug.Assert(7 == system.level);
    }
}
