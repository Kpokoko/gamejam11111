using UnityEngine;
namespace Shooter
{
    public class Spin : MonoBehaviour
    {
        public Vector3 m_Speed = Vector3.zero;
        
        void Update()
        {
            transform.localRotation *= Quaternion.Euler(Time.deltaTime * m_Speed);
        }
    }
}