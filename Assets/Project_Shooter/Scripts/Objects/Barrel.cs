using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shooter.Gameplay
{
    public class Barrel : MonoBehaviour
    {
        [FormerlySerializedAs("m_DamageControl")] [HideInInspector]
        public EnemyHealth mEnemyHealth;

        public GameObject m_ExplodeParticle;

        bool exploded = false;
        // Start is called before the first frame update
        void Start()
        {
            mEnemyHealth = GetComponent<EnemyHealth>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!exploded)
            {
                if (mEnemyHealth.IsDead)
                {
                    exploded = true;
                    Invoke("Explode", .2f);
                }
            }
        }

        public void Explode()
        {
            GameObject obj = Instantiate(m_ExplodeParticle);
            obj.transform.position = transform.position;
            Destroy(obj, 6);
            Destroy(gameObject);
        }
    }
}