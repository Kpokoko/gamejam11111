using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Gameplay
{
    public class CameraControl : MonoBehaviour
    {
        private float m_ShakeTimer;
        private float m_ShakeArc;
        private float m_ShakeRadius = 1;

        public float m_MinZ = 0;

        [HideInInspector]
        public Transform m_Target;
        [HideInInspector]
        public Vector3 m_TargetPoint;
        [SerializeField]
        private Transform m_TargetPointTransform;

        public static CameraControl m_Current;

        public Camera m_MyCamera;

        public Transform m_BossTarget;

        public Transform m_BackBlock;

        [HideInInspector]
        public Vector3 m_CameraBottomPosition;
        [HideInInspector]
        public Vector3 m_CameraTopPosition;

        Vector3 Direction;

        public bool isInit;
        void Awake()
        {
            m_Current = this;
            G.CameraControl = this;
        }

        private void Start()
        {
            G.GameControl.OnGameStart.AddListener(MyStart);
        }

        void MyStart()
        {
            isInit =  true;
            Direction = transform.forward;
            m_MyCamera = GetComponent<Camera>();
            m_MinZ = G.Player.transform.position.z + 10;
            m_CameraBottomPosition = new Vector3(0, 0, -100);
            m_CameraTopPosition = new Vector3(0, 0, -100);

            float distance = 80;
            Direction = Quaternion.Euler(40, 0, 0) * Vector3.forward;
            Vector3 targetPosition = G.Player.transform.position;
            targetPosition.z = m_MinZ;
            targetPosition.x = 0.4f * targetPosition.x;
            transform.position =  targetPosition + -distance * Direction;// - distance// * m_FaceVector;
            transform.forward =  Direction;
        }

        void Update()
        {
            if(!isInit)  return;
            m_ShakeTimer -= Time.deltaTime;

            if (m_ShakeTimer <= 0)
                m_ShakeTimer = 0;

            Vector3 ShakeOffset = Vector3.zero;
            float shakeSin = Mathf.Cos(30 * Time.time) * Mathf.Clamp(m_ShakeTimer, 0, 0.5f);
            float shakeCos = Mathf.Sin(50 * Time.time) * Mathf.Clamp(m_ShakeTimer, 0, 0.5f);
            ShakeOffset = new Vector3(m_ShakeRadius * shakeCos, m_ShakeRadius * shakeSin, 0);

            var zOffset = G.PlayerControl.transform.position.z - m_MinZ;
            m_MinZ = G.PlayerControl.transform.transform.position.z+8;

            float distance = 80;
            Direction = Quaternion.Euler(40, 0, 0) * Vector3.forward;
            Vector3 targetPosition = G.Player.transform.position;
            targetPosition.z = m_MinZ;
            targetPosition.x = 0.4f * targetPosition.x;

            if (m_BossTarget)
            {
                targetPosition = G.Player.transform.position+m_BossTarget.position;
                targetPosition = 0.5f * targetPosition;
                targetPosition.x = 0.6f * targetPosition.x;
            }


            transform.position =Vector3.Lerp(transform.position,  targetPosition + -distance*Direction,5*Time.deltaTime);// - distance// * m_FaceVector;
            transform.position += ShakeOffset;
            transform.forward = Vector3.Lerp(transform.forward, Direction, 5 * Time.deltaTime);

            
            float range = 200;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(0.5f*Screen.width,0,0));
            float dis = 0;
            new Plane(Vector3.up, Vector3.zero).Raycast(ray, out dis);
            m_CameraBottomPosition  = ray.origin + dis * ray.direction;
            m_BackBlock.position = m_CameraBottomPosition;

            ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(0.5f * Screen.width, Screen.height, 0));
            dis = 0;
            new Plane(Vector3.up, Vector3.zero).Raycast(ray, out dis);
            m_CameraTopPosition = ray.origin + dis * ray.direction;

            m_TargetPointTransform.position = m_CameraTopPosition;
        }

        public void StartShake(float t, float r)
        {
            if (m_ShakeTimer == 0 || m_ShakeRadius < r)
                m_ShakeRadius = r;

            m_ShakeTimer = t;
        }
    }
}
