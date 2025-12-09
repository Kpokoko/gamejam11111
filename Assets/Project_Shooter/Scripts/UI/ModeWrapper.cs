using UnityEngine;

public class ModeWrapper : MonoBehaviour
{
    public Mode Mode;
}


public enum Mode
{
    Controls,
    Upgrades,
    Enemies
}