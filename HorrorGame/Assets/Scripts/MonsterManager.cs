using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    public List<ReactToShooting> MonsterList;

    public DoorWife doorWife;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public int MonstersLeft()
    {
        return MonsterList.Count;
    }

    private void OnEnable()
    {
        KillMonster.OnKillMonster += EndGame;
    }

    private void OnDisable()
    {
        KillMonster.OnKillMonster -= EndGame;
    }

    public void EndGame()
    {
        if(MonsterList.Count <= 0)
        {
            doorWife.AllowOpening();
        }
    }
}
