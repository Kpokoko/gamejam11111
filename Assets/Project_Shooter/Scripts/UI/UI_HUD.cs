using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shooter.Gameplay;
using TMPro;

namespace Shooter.UI
{
    public class UI_HUD : MonoBehaviour
    {
        public HealthUI HealthUI;
        public Image m_DamageOverlay;
        public Text[] m_PlayerTexts_1;
        public Text m_GemCountText;
        public Text m_GunNameText;
        public Image m_AimTargetImage;
        public RectTransform m_MainCanvas;

        public Image m_WeaponPowerTime;
        public Image m_PlayerHealth;

        [Space]
        public Image m_BossHealthBase;
        public Image m_BossHealth;

        [Space]
        public Image m_PowerBase;
        public Image m_PowerBar;
        public Text m_PowerNameText;
        public Text m_PowerAmountText;
        public TMP_Text CountEnemies;

        public string[] m_WeaponNames = new string[4] { "PISTOL", "SHOTGUN", "MACHINGUN", "PLASMA GUN" };

        public static UI_HUD m_Main;

        void Awake()
        {
            G.UIHUD = this;
            m_Main = this;
        }
        
        void Start()
        {
            m_BossHealthBase.gameObject.SetActive(false);
        }

        public void UpdatePlayerHealth()
        {
            var h = G.Player.Health;
            HealthUI.UpdateHealthDisplay((int)h.CurrentHealth);
            m_PlayerHealth.fillAmount = h.CurrentHealth / h.MaxHealth;
        }

        void Update()
        {
            m_GemCountText.text = G.PlayerStats.GemCount.ToString();

            if (G.Player.m_TempTarget)
            {
                m_AimTargetImage.gameObject.SetActive(true);
                var v = CameraControl.m_Current.m_MyCamera.WorldToScreenPoint(G.Player.m_TempTarget.m_TargetCenter.position);
                v.x /= Screen.width;
                v.y /= Screen.height;

                v.x = m_MainCanvas.sizeDelta.x * v.x;
                v.y = m_MainCanvas.sizeDelta.y * v.y;

                m_AimTargetImage.rectTransform.anchoredPosition = Helper.ToVector2(v);
            }
            else
            {
                m_AimTargetImage.gameObject.SetActive(false);
            }
            
            m_WeaponPowerTime.fillAmount = G.Player.m_WpnPowerTime / 16f;
            m_GunNameText.text = m_WeaponNames[G.Player.m_WeaponNum];
            

            PlayerPowers p = G.Player.GetComponent<PlayerPowers>();
            if (p.m_HavePower)
            {
                m_PowerBase.gameObject.SetActive(true);
                switch (p.m_PowerNum)
                {
                    case 0:
                        m_PowerNameText.text = "Grenade";
                        m_PowerAmountText.gameObject.SetActive(true);
                        m_PowerAmountText.text = p.m_AmmoCount.ToString();
                        m_PowerBar.gameObject.SetActive(false);
                        break;
                    case 1:
                        m_PowerNameText.text = "Bomb";
                        m_PowerAmountText.gameObject.SetActive(true);
                        m_PowerAmountText.text = p.m_AmmoCount.ToString();
                        m_PowerBar.gameObject.SetActive(false);
                        break;
                }
            }
            else
            {
                m_PowerBase.gameObject.SetActive(false);
            }

        }

        public void ShowBossHealth()
        {
            m_BossHealthBase.gameObject.SetActive(true);
        }
        public void HideBossHealth()
        {
            m_BossHealthBase.gameObject.SetActive(false);
        }
    }
}
