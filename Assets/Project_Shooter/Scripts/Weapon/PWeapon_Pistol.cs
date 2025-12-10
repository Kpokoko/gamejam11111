using UnityEngine;

namespace Shooter.Gameplay
{
    public class PWeapon_Pistol : Weapon_Base
    {
        void Update()
        {
            FireDelayTimer -= Time.deltaTime;
            if (FireDelayTimer <= 0)
                FireDelayTimer = 0;

            RecoilTimer -= 10 * Time.deltaTime;
            if (RecoilTimer <= 0)
                RecoilTimer = 0;
            //Debug.Log("BEFORE");

            if (G.PlayerControl.Input_FireHold)
            {
                //Debug.Log("AFTER");

                if (FireDelayTimer == 0)
                {
                    FireWeapon();
                    FireDelayTimer = FireDelay;
                    RecoilTimer = 1f;
                }
            }

            //Input_FireHold = false;
        }

        public override void FireWeapon()
        {
            GameObject obj;

            if(G.PlayerStats.MultShotUnlock)
            {
                if (PowerLevel == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        obj = Instantiate(BulletPrefab);
                        obj.transform.position = m_FirePoint.position;
                        obj.transform.forward = Quaternion.Euler(0,-6+ i * 6, 0) * m_FirePoint.forward;
                        Projectile_Base proj = obj.GetComponent<Projectile_Base>();
                        proj.Creator = m_Owner;
                        proj.Speed = ProjectileSpeed;
                        proj.Damage = Damage;
                        proj.m_Range = Range;
                        Destroy(obj, 5);
                    }

                }
            }
            else if (PowerLevel == 0)
            {
                obj = Instantiate(BulletPrefab);
                obj.transform.position = m_FirePoint.position;
                obj.transform.forward = m_FirePoint.forward;
                Projectile_Base proj = obj.GetComponent<Projectile_Base>();
                proj.Creator = m_Owner;
                proj.Speed = ProjectileSpeed;
                proj.Damage = Damage;
                proj.m_Range = Range;
                Destroy(obj, 5);
            }
            else if (PowerLevel == 1)
            {
                for (int i = -1; i < 2; i++)
                {
                    obj = Instantiate(BulletPrefab);
                    obj.transform.position = m_FirePoint.position;
                    obj.transform.forward = Quaternion.Euler(0, i * 10, 0) * m_FirePoint.forward;
                    Projectile_Base proj = obj.GetComponent<Projectile_Base>();
                    proj.Creator = m_Owner;
                    proj.Speed = ProjectileSpeed;
                    proj.Damage = Damage;
                    proj.m_Range = Range;
                    Destroy(obj, 5);
                }
            }

            obj = Instantiate(EffectPrefab);
            obj.transform.SetParent(m_ParticlePoint);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.forward = m_ParticlePoint.forward;
            Destroy(obj, 3);
        }
    }
}