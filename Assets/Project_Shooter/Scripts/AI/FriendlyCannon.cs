using System.Collections;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class FriendlyCannon : Enemy
    {
        public GameObject m_BulletPrefab1;
        public GameObject m_FireParticlePrefab1;
        public GameObject m_PreFireParticlePrefab1;
        public GameObject _currentTarget;
        void Start()
        {
            StartCoroutine(Co_AttackLoop());
        }
        
        void Update()
        {
            FaceTarget();
            HandleDeath();
        }

        IEnumerator Co_AttackLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                var otherObjects = Physics.OverlapSphere(transform.position, 30f);
                GameObject nearestObj = null;
                var bestDistance = float.MaxValue;
                foreach (var obj in otherObjects)
                {
                    if (obj.gameObject == gameObject)
                        continue;
                    if (!obj.gameObject.CompareTag("Enemy"))
                        continue;
                    var dist = Vector3.Distance(transform.position, obj.transform.position);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        nearestObj = obj.gameObject;
                    }
                }
                _currentTarget = nearestObj;
                if (_currentTarget)
                {
                    GameObject particle = Instantiate(m_PreFireParticlePrefab1);
                    particle.transform.SetParent(m_FirePoint);
                    particle.transform.localPosition = Vector3.zero;
                    Destroy(particle, 3);

                    yield return new WaitForSeconds(1.3f);
                    ShootBullet();
                }

                yield return new WaitForSeconds(1.5f);

            }
        }

        public void ShootBullet()
        {
            GameObject obj = Instantiate(m_BulletPrefab1);
            obj.transform.position = m_FirePoint.position + new Vector3(0, 1.0f, 0);
            Vector3 dir = _currentTarget.transform.position - transform.position;
            dir.y = 0;
            obj.transform.forward = dir;
            obj.GetComponent<ProjectileCollision>().m_Creator = gameObject;
            Destroy(obj, 10);

            obj = Instantiate(m_FireParticlePrefab1);
            obj.transform.position = m_FirePoint.position;
            Destroy(obj, 3);
        }
        
        public void FaceTarget()
        {
            if (_currentTarget)
            {
                Vector3 dir = _currentTarget.transform.position - transform.position;
                dir.y = 0; // игнорируем вертикаль

                dir.Normalize();
                m_RotationBase.rotation = Quaternion.Lerp(
                    m_RotationBase.rotation,
                    Quaternion.LookRotation(dir),
                    10f * Time.deltaTime
                );
            }
        }
    }
}
