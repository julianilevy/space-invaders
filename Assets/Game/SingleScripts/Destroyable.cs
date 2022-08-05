using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public class Destroyable : MonoBehaviour
    {
        public int health = 1;
        public AudioSource audioSource = null;

        public Action<int> HealthSetted;
        public Action Damaged;
        public Action HealthDrained;

        public void SetHealth(int health)
        {
            this.health = health;

            HealthSetted?.Invoke(health);
        }

        public void Damage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                if (audioSource != null)
                    audioSource.Play();
                HealthDrained?.Invoke();
            }
            else
                Damaged?.Invoke();
        }

        public void DrainFullHealth()
        {
            if (audioSource != null)
                audioSource.Play();
            health = 0;
            HealthDrained?.Invoke();
        }
    }
}