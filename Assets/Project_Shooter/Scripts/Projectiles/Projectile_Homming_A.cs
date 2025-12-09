using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Projectile_Homming_A : MonoBehaviour
    {
        bool m_Chase = true;
        void Start()
        {
            m_Chase = true;
            Invoke("StopChase", 4);
        }

        void Update()
        {
            if (m_Chase)
            {
                Vector3 dir = G.Player.transform.position - transform.position;
                dir.y = 0;
                transform.forward = dir;
            }
        }

        void StopChase()
        {
            m_Chase = false;
        }
    }
}