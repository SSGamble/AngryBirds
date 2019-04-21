using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黄色小鸟，继承自小鸟
/// </summary>
public class YellowBird : Bird {

    /// <summary>
    /// 重写施展技能的方法
    /// </summary>
    public override void ShowSkill()
    {
        base.ShowSkill();
        rg.velocity *= 2;
    }
}
