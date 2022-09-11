using UnityEngine;


public class BattleManager : MonoBehaviour
{
    public static bool battleOn { get; private set; }
    public static bool bossBattleOn { get; private set; }

    public static event System.Action<bool> OnBattleStateChange;
    public static void BattleStart(){
        battleOn = true;

        OnBattleStateChange?.Invoke(true);
    }

    public static void BattleEnd(){
        battleOn = false;

        OnBattleStateChange?.Invoke(false);
    }

    public static event System.Action<bool> OnBossBattleStateChange;

    public static void BossBattleStart()
    {
        battleOn = true;

        OnBossBattleStateChange?.Invoke(true);
    }

    public static void BossBattleEnd()
    {
        battleOn = false;

        OnBossBattleStateChange?.Invoke(false);
    }
}