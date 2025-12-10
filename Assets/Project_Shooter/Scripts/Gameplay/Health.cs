using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Shooter.Gameplay
{
    public class Health : MonoBehaviour
    {
        public float CurrentHealth = 11;
        public float MaxHealth = 11;

        [HideInInspector] public bool IsDead = false;
        [HideInInspector] public bool m_NoDamage = false;

        [HideInInspector] public Vector3 LastDamageDirection;
        [HideInInspector] public float LastDamageFactor = 1;

        [HideInInspector] public float DamageShakeAmount;
        [HideInInspector] public float DamageShakeAngle;

        public UnityEvent OnDamaged = new();
        public UnityEvent OnDeath = new();


        void Awake()
        {
            OnDamaged = new UnityEvent();
        }

        void Start()
        {
            CurrentHealth = MaxHealth;
            IsDead = false;
            LastDamageDirection = Vector3.forward;
            DamageShakeAmount = 0;
            DamageShakeAngle = 0;
        }


        void Update()
        {
            DamageShakeAmount -= 12 * Time.deltaTime;
            if (DamageShakeAmount <= 0)
                DamageShakeAmount = 0;
        }

        public void ApplyDamage(float dmg, Vector3 dir, float DamageFactor)
        {
            if (m_NoDamage || IsDead)
                return;

            ApplyDamageShake();
            LastDamageDirection = dir;
            LastDamageDirection.Normalize();
            LastDamageFactor = DamageFactor;
            CurrentHealth -= dmg;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                IsDead = true;
                OnDeath.Invoke();
            }

            OnDamaged.Invoke();
            StartCoroutine(Co_HitGlow());
        }

        public IEnumerator Co_HitGlow()
        {
            HitGlowControl[] glowControls = GetComponentsInChildren<HitGlowControl>();
            foreach (HitGlowControl item in glowControls)
            {
                item.SetGlow();
            }

            yield return new WaitForSeconds(.1f);

            foreach (HitGlowControl item in glowControls)
            {
                item.SetOriginal();
            }


            yield return null;
        }

        public void AddHealth(float h)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + h, 0, MaxHealth);
            G.UIHUD.UpdatePlayerHealth();
        }

        public void ApplyDamageShake()
        {
            if (DamageShakeAmount == 0)
            {
                DamageShakeAmount = 1;
                DamageShakeAngle = Random.Range(-1f, 1f);
            }
        }

        // public void Reset()
        // {
        //     CurrentHealth = MaxHealth;
        //     IsDead = false;
        // }
    }
}