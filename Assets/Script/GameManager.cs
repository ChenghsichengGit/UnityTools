using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Ongoing, Pause
}

public static class GameManager
{
    public static GameState GameState;
}
