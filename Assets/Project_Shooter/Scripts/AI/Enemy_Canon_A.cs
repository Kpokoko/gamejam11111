using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Enemy_Canon_A : Enemy
    {
        public GameObject m_BulletPrefab1;
        public GameObject m_FireParticlePrefab1;
        public GameObject m_PreFireParticlePrefab1;
        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            
            health.OnDeath.AddListener(SendAboutDeath);
            StartCoroutine(Co_AttackLoop());
        }
        
        void SendAboutDeath()
        {
            G.LevelController.OnEnemyDied();
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(transform.position, G.Player.transform.position) <= 30f)
            {
                m_FacePlayer = true;
            }
            else
            {
                m_FacePlayer = false;
            }

            HandleFacePlayer();
            HandleDeath();
        }

        IEnumerator Co_AttackLoop()
        {
           
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                if (Vector3.Distance(transform.position, G.Player.transform.position) <= 30f)
                {
                    GameObject obj = Instantiate(m_PreFireParticlePrefab1);
                    obj.transform.SetParent(m_FirePoint);
                    obj.transform.localPosition = Vector3.zero;
                    Destroy(obj, 3);

                    yield return new WaitForSeconds(1.3f);
                    ShootBullet();
                }
                yield return new WaitForSeconds(1.5f);

            }
        }

        public void ShootBullet()
        {
            GameObject obj = Instantiate(m_BulletPrefab1);
            obj.transform.position = m_FirePoint.position;
            Vector3 dir = G.Player.transform.position - transform.position;
            dir.y = 0;
            obj.transform.forward = dir;
            obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
            Destroy(obj, 10);

            obj = Instantiate(m_FireParticlePrefab1);
            obj.transform.position = m_FirePoint.position;
            Destroy(obj, 3);
        }
    }
}
