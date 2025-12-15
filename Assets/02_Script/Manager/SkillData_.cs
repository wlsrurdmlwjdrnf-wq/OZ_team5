using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Active = 1,
    Passive
}


public class SkillData_
{
    public int id;
    public string name;
    public SkillType type;
    public int maxLv;
    public float dmg;
    public float coolTime;
}
