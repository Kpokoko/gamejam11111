using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shooter.Gameplay
{
    public class DestructObject : MonoBehaviour
    {
        [FormerlySerializedAs("MyDamageControl")] [HideInInspector]
        public EnemyHealth myEnemyHealth;

        public GameObject BrakeEffectPrefab1;
        public GameObject DestroyedObjectPrefab1;
        public Transform m_ExplodeCenter;

        Quaternion InitRotation;
        Vector3 InitPosition;

        //public bool ShakePosition = false;
        //public bool ShakeRotation = true;
        //public bool ShakeColor = true;
        //public bool CanDestroy = true;


        public float ShakeRadius = 10;
        // Use this for initialization
        void Start()
        {
            myEnemyHealth = GetComponent<EnemyHealth>();
            InitRotation = transform.rotation;
            InitPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            //if (ShakeRotation)
            //{
            //    transform.rotation = InitRotation * Quaternion.Euler(0, 0, ShakeRadius * MyDamageControl.DamageShake * Mathf.Sin(5 * Time.time));
            //}

            //if (ShakePosition)
            //{
            //    transform.position = InitPosition + new Vector3(0, ShakeRadius * MyDamageControl.DamageShake * Mathf.Sin(5 * Time.time), 0);
            //}

            //if (ShakeColor)
            //{

            //    MainSprite.color = Color.Lerp(Color.white, Color.red, MyDamageControl.DamageShake);
            //}

            //if (CanDestroy)
            {
                if (myEnemyHealth.IsDead)
                {
                    GameObject obj = Instantiate(BrakeEffectPrefab1);
                    if (m_ExplodeCenter!=null)
                        obj.transform.position = m_ExplodeCenter.position;
                    else
                        obj.transform.position = transform.position;
                    //obj.transform.rotation = MyDamageControl.LastDamageDirection
                    Destroy(obj, 4);

                    if (DestroyedObjectPrefab1 != null)
                    {
                        obj = Instantiate(DestroyedObjectPrefab1);
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation;
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}