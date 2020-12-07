using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int lives;
    public int resetLives;

    public PlayerData(Player player)
    {
        level = player.currentLevel();
        lives = player.GetLives();
        resetLives = player.GetResetLives();
    }
}
