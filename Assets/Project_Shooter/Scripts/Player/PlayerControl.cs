using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shooter.Gameplay
{
    public class PlayerControl : MonoBehaviour
    {
        [HideInInspector]
        public PlayerChar MyPlayerChar;
        [HideInInspector]
        public GameObject MyPlayerPlane;

        public Transform m_SpawnPoint;

        [HideInInspector]
        public int m_GemCount = 0;

        [HideInInspector]
        public PlayerControl OtherControl;

        public GameObject PlayerPrefab1;
        //public GameObject PlayerPlanePrefab1;

        public int ControllerNum = 0;
        [HideInInspector]
        public string PlayerName = "Player";


        [HideInInspector]
        public int Kills = 0;

        public bool m_IsDead = false;

        [HideInInspector]
        public Vector3 ReticlePosition;

        [HideInInspector]
        public Weapon_Base[] MainWeapons = new Weapon_Base[2];
        [HideInInspector]
        public int CurrentWeaponNum = 0;
        [HideInInspector]
        public Weapon_Base CurrentWeapon;


        [HideInInspector]
        public Transform NextSpawnPoint;

        [HideInInspector]
        public Vector3 LastDeathPosition;

        bool SelectedSpawnPoint = false;

        public static PlayerControl MainPlayerController;

        [HideInInspector]
        public int State = 0;
        [HideInInspector]
        public float StateStartTime = 0;

        [HideInInspector]
        public bool UsingPowerWeapon = false;


        [HideInInspector]
        public bool InputEnable = true;
        [HideInInspector]
        public Vector3 m_Input_Movement;
        [HideInInspector]
        public Vector3 AimPosition;
        [HideInInspector]
        public Vector2 m_CameraAngle;
        [HideInInspector]
        public bool Input_Fire = false;
        [HideInInspector]
        public bool Input_FireHold = false;
        [HideInInspector]
        public bool Input_HoldAim = false;
        [HideInInspector]
        public bool Input_Grenade = false;
        [HideInInspector]
        public bool Input_Force = false;
        [HideInInspector]
        public bool Input_Dash = false;
        [HideInInspector]
        public bool Input_ChangeWeapon = false;
        [HideInInspector]
        public bool Input_Interact = false;
        [HideInInspector]
        public bool Input_Fly = false;
        [HideInInspector]
        public bool[] Input_Detonate;

        [Space]
        public Transform m_AimPointTransofrm;


        [HideInInspector]
        public bool m_IsOnFoot = false;

        //[HideInInspector]
        //public PlayerInvetory m_Inventory;

        //[SerializeField, Space]
        //private DataStorage m_DataStorage;
        //[SerializeField, Space]
        //private Content m_Contents;


        void Awake()
        {
            MainPlayerController = this;
        }

        void Start()
        {
            InputEnable = true;
            MainWeapons = new Weapon_Base[2];
            

            State = 0;
            StateStartTime = Time.time;
            InputEnable = true;
            m_IsOnFoot = true;
            Respawn();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateInputs();

            if (m_IsDead) return;
            if (!PlayerChar.m_Current.m_IsDead) return;
            m_IsDead = true;
            G.GameControl.HandlePlayerDeath();
        }

        public void UpdateInputs()
        {
            m_Input_Movement = Vector3.zero;
            Input_Fire = false;
            Input_FireHold = false;
            Input_Interact = false;
            Input_HoldAim = false;
            Input_Grenade = false;
            Input_Fly = false;
            Input_Dash = false;
            Input_ChangeWeapon = false;
            Input_Detonate = new bool[4];

            Vector3 cameraForward = CameraControl.m_Current.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 cameraRight = Helper.RotatedVector(90, cameraForward);

            if (Input.GetKey(KeyCode.UpArrow))
            {
                m_Input_Movement += cameraForward;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                m_Input_Movement -= cameraForward;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                m_Input_Movement -= cameraRight;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                m_Input_Movement += cameraRight;
            }

            if (Input.GetKey(KeyCode.Z))
            {
                Input_FireHold = true;
            }


            Ray ray = CameraControl.m_Current.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            float dis = 0;
            new Plane(Vector3.up, Vector3.zero).Raycast(ray, out dis);
            AimPosition = ray.origin + dis * ray.direction;
            m_AimPointTransofrm.position = AimPosition;
            ReticlePosition = m_AimPointTransofrm.position;
        }
       
        
        
        public void Respawn()
        {
            var obj = Instantiate(PlayerPrefab1);
            MyPlayerChar = obj.GetComponent<PlayerChar>();

            if (G.GameControl.m_MainSaveData.m_CheckpointNumber == 0)
            {
                MyPlayerChar.transform.position = m_SpawnPoint.position + new Vector3(0, .1f, 0);
            }
            else
            {
                int num = G.GameControl.m_MainSaveData.m_CheckpointNumber - 1;
                MyPlayerChar.transform.position = CheckpointControl.m_Main.m_Checkpoints[num].m_SpawnPoint.position;
            }

            State = 1;
            StateStartTime = Time.time;
        }
    }
}