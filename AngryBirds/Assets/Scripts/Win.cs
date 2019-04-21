using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 赢
/// </summary>
public class Win : MonoBehaviour {

    /// <summary>
    /// 显示星星数，在动画播放完添加的事件
    /// </summary>
    public void Show()
    {
        GameManager.Instance.ShowStart();
    }
}
