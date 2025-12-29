using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SupportSkillBase : MonoBehaviour
{
    protected IngameItemData supportSkillData;
    protected Player player;

    public abstract int Id { get; set; }
    public int Level { get {  return level; } }
    protected int level;

    protected void Awake()
    {
        supportSkillData = DataManager.Instance.GetIngameItemData(Id);
        level = supportSkillData.level;
    }
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    public abstract void LevelUp();
}
