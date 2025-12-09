using System.Collections.Generic;
using Shooter.Gameplay;

public static class G
{
    public static LevelController LevelController;
    public static PlayerChar Player;
    public static PlayerStats PlayerStats;
    public static GameControl  GameControl;
}

public class PlayerStats
{
    public Dictionary<BaseUpgradeUIData, int> UpgradeData;
}