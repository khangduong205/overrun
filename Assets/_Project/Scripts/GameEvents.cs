using System;

public static class GameEvents
{
    public static event Action<int> LevelUp;  
    public static void RaiseLevelUp(int level) => LevelUp?.Invoke(level);
}