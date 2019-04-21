using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回旋小鸟，继承自 Bird
/// </summary>
public class GreenBird : Bird {

    /// <summary>
    /// 回旋
    /// </summary>
    public override void ShowSkill()
    {
        base.ShowSkill();
        Vector3 speed = rg.velocity;
        speed.x *= -1; // 反转位置
        rg.velocity = speed;
    }
}
