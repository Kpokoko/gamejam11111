using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shooter.Gameplay
{
    public class Barrel : MonoBehaviour
    {
        [FormerlySerializedAs("mEnemyHealth")] [FormerlySerializedAs("m_DamageControl")] [HideInInspector]
        public Health mHealth;

        public GameObject m_ExplodeParticle;

        bool exploded = false;
        // Start is called before the first frame update
        void Start()
        {
            mHealth = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!exploded)
            {
                if (mHealth.IsDead)
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