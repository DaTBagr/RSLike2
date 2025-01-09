using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemySkeleton : NPC
{
    public override void Start()
    {
        base.Start();
        
        SetHealth(100);
        SetHostile(true);
        SetSpeed(3);
        SetAttackRange(1);
        SetDetectionRange(3);
    }
}
