using Shooter;
using Shooter.Gameplay;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [HideInInspector] public Vector3 Input_Movement;
    [HideInInspector] public Vector3 AimPosition;

    private Transform _aimPointTransofrm;

    public bool Input_Fire;
    public bool Input_FireHold;
    public bool Input_Dash;

    void Awake()
    {
        _aimPointTransofrm = new GameObject("Aim").GetComponent<Transform>();
        G.PlayerControl = this;
    }

    void Update()
    {
        UpdateInputs();
    }

    public void UpdateInputs()
    {
        if(!G.LevelController.isLevelRunning) return;
        Input_Movement = Vector3.zero;
        Input_Fire = false;
        Input_FireHold = false;
        Input_Dash = false;

        Vector3 cameraForward = CameraControl.m_Current.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = Helper.RotatedVector(90, cameraForward);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Input_Movement += cameraForward;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Input_Movement -= cameraForward;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Input_Movement -= cameraRight;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Input_Movement += cameraRight;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            Input_FireHold = true;
        }

        if (Input.GetKey(KeyCode.X))
        {
            if(G.PlayerStats.DashUnlock) 
                Input_Dash = true;
        }


        Ray ray = CameraControl.m_Current.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        float dis = 0;
        new Plane(Vector3.up, Vector3.zero).Raycast(ray, out dis);
        AimPosition = ray.origin + dis * ray.direction;
        _aimPointTransofrm.position = AimPosition;
    }
}