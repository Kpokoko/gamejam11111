using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class ProjectileCollision : MonoBehaviour
    {
        public GameObject m_Creator;
        public GameObject m_HitParticle;
        public float m_Damage = 1;
        public bool m_IsEnemyTeam = true;

        void Update()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);
            foreach (Collider col in hits)
            {
                if (col.gameObject == m_Creator)
                    continue;

                if (col.gameObject.tag == "Player" && m_IsEnemyTeam)
                {
                    Health d = col.gameObject.GetComponent<Health>();
                    if (d != null)
                    {
                        if(d.IsDead) return;
                        if (!d.isActiveAndEnabled) return;
                        d.ApplyDamage(m_Damage, transform.forward, 1);
                    }
                    PlayerChar p = col.gameObject.GetComponent<PlayerChar>();
                    CreateHitParticle();
                    Destroy(gameObject);

                }
                else if (col.gameObject.tag == "Block")
                {
                    Health d = col.gameObject.GetComponent<Health>();
                    if (d != null)
                    {
                        d.ApplyDamage(m_Damage, transform.forward, 1);
                    }
                    CreateHitParticle();
                    Destroy(gameObject);
                }
                else if (col.gameObject.tag == "Enemy" && m_IsEnemyTeam)
                {
                    Health d = col.gameObject.GetComponent<Health>();
                    if (d != null)
                    {
                        d.ApplyDamage(m_Damage, transform.forward, 1);
                    }
                    CreateHitParticle();
                    Destroy(gameObject);
                }

            }
        }

        public void CreateHitParticle()
        {
            GameObject obj = Instantiate(m_HitParticle);
            obj.transform.position = transform.position;
            Destroy(obj, 3);
        }
    }
}
