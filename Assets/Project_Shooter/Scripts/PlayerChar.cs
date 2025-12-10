using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Gameplay
{
    public class PlayerChar : MonoBehaviour
    {
        [SerializeField] private Transform m_TurnBase;
        [SerializeField] private Transform m_AimBase;

        [HideInInspector] public CameraControl m_MyCamera;

        [HideInInspector] public bool m_InControl = false;
        [HideInInspector] public bool m_CanShoot = false;

        public Transform[] m_WeaponHands;
        public Transform m_FirePoint;
        public GameObject m_WeaponPowerParticle;
        public GameObject m_DeathParticle;

        Vector3 m_MovementInput;
        Vector3 m_DashDirection;

        public AnimationCurve m_DashCurve;
        public GameObject m_DashParticle;

        public GameObject m_HitParticlePrefab;

        public Weapon_Base[] Weapons;
        [HideInInspector] public int m_WeaponNum = 0;

        public TargetObject m_TempTarget;

        [HideInInspector] public int m_WpnPowerLevel = 0;
        [HideInInspector] public float m_WpnPowerTime = 0;

        public Animator m_Animator;
        public GameObject m_GrenadePrefab1;
        public GameObject m_ShieldObject;

        public Health Health;
        public PlayerPowers PlayerPowers;
        public PlayerControl PlayerControl;
        public PlayerStats PlayerStats;
        public Rigidbody Rigidbody;

        // -------------------------
        // DASH COOLDOWN
        // -------------------------
        [SerializeField] private float dashCooldown = 2f;
        private float dashCooldownTimer = 0f;
        // -------------------------

        void Awake()
        {
            Health = GetComponent<Health>();
            PlayerPowers = GetComponent<PlayerPowers>();
            PlayerControl = GetComponent<PlayerControl>();
            PlayerStats = GetComponent<PlayerStats>();
            Rigidbody = GetComponent<Rigidbody>();
            G.Player = this;
        }

        void Start()
        {
            Health.OnDamaged.AddListener(HandleDamage);
            Health.OnDamaged.AddListener(G.UIHUD.UpdatePlayerHealth);
            Health.OnDeath.AddListener(DeathHandler);

            m_InControl = true;

            m_ShieldObject.transform.SetParent(null);
            m_WeaponPowerParticle.SetActive(false);
            G.UIHUD.UpdatePlayerHealth();
        }

        public void DeathHandler()
        {
            if (!(Health.CurrentHealth <= 0)) return;
            G.GameControl.OnGameOver.Invoke();

            var obj = Instantiate(m_DeathParticle);
            obj.transform.position = transform.position + new Vector3(0, 1, 0);
            Destroy(obj, 3);
            gameObject.SetActive(false);
        }

        void Update()
        {
            // Dash cooldown tick
            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;

            if (m_InControl)
            {
                UpdateInput();
                UpdateSoftAiming();

                if (m_MovementInput != Vector3.zero)
                {
                    Vector3 faceDirection = m_MovementInput;
                    faceDirection.y = 0;
                    faceDirection.Normalize();
                    m_TurnBase.rotation = Quaternion.Lerp(m_TurnBase.rotation, Quaternion.LookRotation(faceDirection),
                        10 * Time.deltaTime);
                }
            }

            if (m_WpnPowerLevel == 1)
            {
                m_WpnPowerTime -= Time.deltaTime;
                if (m_WpnPowerTime <= 0)
                {
                    m_WeaponPowerParticle.SetActive(false);
                    SetWeaponPowerLevel(0);
                }
            }

            var vSpeed = Rigidbody.linearVelocity;
            vSpeed.y = 0;
            var runSpeed = Mathf.Clamp(vSpeed.magnitude / 10f, 0, 1);
            m_Animator.SetFloat("RunSpeed", runSpeed);

            m_ShieldObject.transform.position = transform.position + new Vector3(0, 1, 0);
        }

        private void UpdateInput()
        {
            Weapons[m_WeaponNum].Input_FireHold = G.PlayerControl.Input_FireHold;

            if (PlayerControl.Input_Dash)
                StartDash();

            m_MovementInput = G.PlayerControl.Input_Movement;
        }

        void UpdateSoftAiming()
        {
            var targets = TargetsControl.m_Main.m_Targets;

            TargetObject bestTarget = null;

            float minAngle = 15f;
            if (PlayerStats.SoftAimActive)
            {
                minAngle = 40f;
            }

            foreach (var target in targets)
            {
                if (target == null)
                    continue;

                Vector3 targetPos = target.m_TargetCenter.position;
                Vector3 dir = targetPos - transform.position;
                dir.y = 0;
                var delta = Vector3.Angle(m_TurnBase.forward, dir);
                var distance = dir.magnitude;

                if (distance > 30)
                    continue;

                if (delta < minAngle)
                {
                    bestTarget = target;
                    minAngle = delta;
                }
            }

            if (bestTarget)
            {
                var targetPos = bestTarget.m_TargetCenter.position;
                var targetDir = targetPos - m_FirePoint.position;
                targetDir.y = 0;

                var rotationSpeed = PlayerStats.SoftAimActive ? 30f : 20f;

                m_AimBase.rotation = Quaternion.Lerp(m_AimBase.rotation, Quaternion.LookRotation(targetDir),
                    rotationSpeed * Time.deltaTime);
                m_TempTarget = bestTarget;
            }
            else
            {
                m_TempTarget = null;
                m_AimBase.localRotation =
                    Quaternion.Lerp(m_AimBase.localRotation, Quaternion.identity, 20 * Time.deltaTime);
            }
        }

        void FixedUpdate()
        {
            var totalVelocity = Rigidbody.linearVelocity;
            if (m_MovementInput != Vector3.zero)
            {
                totalVelocity += 5 * m_MovementInput;
                totalVelocity.y = 0;
                totalVelocity = Vector3.ClampMagnitude(totalVelocity, 11);
                totalVelocity.y = Rigidbody.linearVelocity.y;
                Rigidbody.linearVelocity = totalVelocity;
            }
            else
            {
                totalVelocity -= .4f * totalVelocity;
                totalVelocity.y = Rigidbody.linearVelocity.y;
                Rigidbody.linearVelocity = totalVelocity;
            }
        }

        public void HandleDamage()
        {
            CameraControl.m_Current.StartShake(.2f, .1f);
        }

        void LateUpdate()
        {
            float recoil = Weapons[m_WeaponNum].RecoilTimer;
            m_WeaponHands[0].localRotation *= Quaternion.Euler(0, -4 * recoil, 0);
            m_WeaponHands[1].localRotation *= Quaternion.Euler(0, -4 * recoil, 0);
            m_WeaponHands[0].localPosition += new Vector3(0, 0, -.5f * recoil);
        }

        public void CheckMelleeAttack()
        {
            var colls = Physics.OverlapSphere(transform.position + 2 * m_AimBase.forward, 1);
            foreach (var col in colls)
            {
                if (col.gameObject.tag == "Enemy")
                {
                    var d = col.gameObject.GetComponent<Health>();
                    if (d != null)
                    {
                        Vector3 dir = col.gameObject.transform.position - transform.position;
                        dir.Normalize();
                        d.ApplyDamage(5, dir, 1);
                    }
                }
                else if (col.gameObject.tag == "Block")
                {
                    var health = col.gameObject.GetComponent<Health>();
                    if (health)
                    {
                        health.ApplyDamage(1, transform.forward, 1);
                    }
                }
            }
        }

        public void SetWeaponPowerLevel(int level)
        {
            m_WpnPowerLevel = level;
            if (level == 1)
            {
                m_WpnPowerTime = 16;
                m_WeaponPowerParticle.SetActive(true);
            }

            foreach (var w in Weapons)
            {
                w.PowerLevel = level;
            }
        }

        public void SetWeapon(int num)
        {
            foreach (Weapon_Base w in Weapons)
            {
                w.Input_FireHold = false;
            }

            m_WeaponNum = num;
        }

        public void ThrowGrenade()
        {
            var start = transform.position;
            var end = G.PlayerControl.AimPosition + new Vector3(0, 1, 0);
            var obj = Instantiate(m_GrenadePrefab1);
            obj.transform.position = transform.position;
            var g = obj.GetComponent<PlayerGrenade>();
            g.m_StartPosition = start;
            g.m_TargetPosition = end;
        }

        public void HandlePickup(string itemType, int count)
        {
            if (itemType == "Gem")
            {
                G.PlayerStats.GemCount++;
            }
            else if (itemType == "WeaponPower")
            {
                SetWeaponPowerLevel(1);
            }
            else if (itemType == "Weapon_Pistol")
            {
                SetWeapon(0);
            }
            else if (itemType == "Weapon_Shotgun")
            {
                SetWeapon(1);
            }
            else if (itemType == "Power_Grenade")
            {
                PlayerPowers.SetNewPower(0);
            }
            else if (itemType == "Health")
            {
                Health.AddHealth(count);
            }
        }

        public void StartDash()
        {
            if (dashCooldownTimer > 0) return; // Кулдаун ещё идёт

            m_DashDirection = m_MovementInput;
            if (m_DashDirection != Vector3.zero)
            {
                dashCooldownTimer = dashCooldown; // ставим кулдаун
                StartCoroutine(Co_Dash());
            }
        }

        IEnumerator Co_Dash()
        {
            var obj = Instantiate(m_DashParticle);
            obj.transform.position = transform.position + new Vector3(0, 1, 0);
            obj.transform.forward = m_DashDirection;
            Destroy(obj, 3);

            GetComponent<Collider>().enabled = false;
            m_Animator.SetBool("Dashing", true);
            GetComponent<Rigidbody>().isKinematic = true;
            float lerp = 0;
            Vector3 startPos = transform.position;
            Vector3 endPos = transform.position + 6 * m_DashDirection;
            while (lerp < 1)
            {
                transform.position = Vector3.Lerp(startPos, endPos, m_DashCurve.Evaluate(lerp));
                lerp += 4 * Time.deltaTime;
                yield return null;
            }

            transform.position = endPos;
            GetComponent<Rigidbody>().isKinematic = false;
            m_DashDirection = Vector3.zero;
            m_Animator.SetBool("Dashing", false);
            GetComponent<Collider>().enabled = true;
        }

        public void Hit()
        {
            m_MyCamera.StartShake(.5f, .5f);
            GameObject obj = Instantiate(m_HitParticlePrefab);
            obj.transform.position = transform.position;
            Destroy(obj, 1);
        }
    }
}
