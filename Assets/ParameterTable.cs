using UnityEngine;

[CreateAssetMenu( menuName = "MyGame/Create ParameterTable", fileName = "ScriptableObject" )]
public class BattleCharacterObject : ScriptableObject
{
    public int[] skillIndex;
    public int HP;
    public int MP;
    public int Atk;
    public int Def;
    public int Speed;
} // class ParameterTable