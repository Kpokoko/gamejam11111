using System.Collections;
using UnityEngine;

namespace Shooter.Gameplay
{
    public class SpiderSpawner_A : MonoBehaviour
    {
        public Transform m_Base;
        public Transform m_SpawnPoint;
        public GameObject m_EnemyPrefab1;
        public Health health;

        void Start()
        {
            health = GetComponent<Health>();
            m_Base.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(Co_StartSpawn());

            health.OnDeath.AddListener(Death);
        }

        void Death()
        {
            G.LevelController.OnEnemyDied();
            Destroy(gameObject);
        }

        IEnumerator Co_StartSpawn()
        {
            yield return new WaitForSeconds(2);
            float lerp = 0;
            while (lerp <= 1)
            {
                m_Base.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 3.8f, 0), lerp);
                m_Base.localPosition += new Vector3(0.03f * Mathf.Sin(50 * Time.time), 0, 0);
                lerp += 0.6f * Time.deltaTime;
                yield return null;
            }

            m_Base.localPosition = new Vector3(0, 3.8f, 0);
            yield return new WaitForSeconds(.5f);

            // ——— Бесконечный спавн пачками ———
            while (true)
            {
                // Спавним 3 врага
                for (int i = 0; i < 2; i++)
                {
                    SpawnOne();
                    yield return new WaitForSeconds(1); // интервал внутри пачки
                }

                yield return new WaitForSeconds(8); // интервал между пачками
            }
        }

        void SpawnOne()
        {
            GameObject obj = Instantiate(m_EnemyPrefab1);
            obj.transform.position = m_SpawnPoint.position;
            obj.transform.forward = Vector3.back;

            Enemy e = obj.GetComponent<Enemy>();
            e.isClone = true;
            e.m_StartWalkDistance = 10;
        }
    }
}