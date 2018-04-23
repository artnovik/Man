using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat endurance; //HP
    public Stat agility;   //MoveSpeed
    public Stat stamina;   //SP
    public Stat strength;  //DMG

    // Magic
    public Stat arcane;    //Can do magic, but are also more resistant to magic
    public Stat fire;      //Weak to water damage/strong to fire damage
    public Stat water;     //weak to fire damage/strong to water damage
}
