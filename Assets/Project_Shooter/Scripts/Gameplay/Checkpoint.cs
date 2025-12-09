using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class Checkpoint : MonoBehaviour
    {
        public int m_CheckpointNumber = 0;
        [HideInInspector]
        public bool m_IsActivated = false;
        public Transform m_SpawnPoint;
        public GameObject[] m_Bases;
        public GameObject m_ActivateParticle;
       

        void Update()
        {
            if (!m_IsActivated)
            {
                if (Vector3.Distance(PlayerChar.m_Current.transform.position,transform.position)<=6)
                {
                    Activate();
                }
            }
        }

        public void Activate()
        {
            if (m_IsActivated) return;
            
            m_IsActivated = true;
            m_Bases[0].SetActive(false);
            m_Bases[1].SetActive(true);
            G.GameControl.HandleCheckpoint(m_CheckpointNumber);

            var obj = Instantiate(m_ActivateParticle);
            obj.transform.position = transform.position + new Vector3(0, .3f, 0);
            Destroy(obj, 3);
        }
    }
}
