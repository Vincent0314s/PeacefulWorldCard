using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRuleData", menuName = "Utility/Game Rule")]
public class GameBoardRuleSO : ScriptableObject
{
    public int FlagMaximumNumber = 3;
    public int ShieldMaximumNumber = 5;
    public int CannonMaximumNumber = 3;
}
