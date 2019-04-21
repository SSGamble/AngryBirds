using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 炸弹小鸟，继承小鸟
/// 	会炸掉自身周围的物体
/// </summary>
public class BlackBird : Bird {

    private List<Pig> blockList = new List<Pig>();

    /// <summary>
    /// 重写：爆炸效果
    /// </summary>
    public override void ShowSkill()
    {
        base.ShowSkill(); // 基类中的鼠标事件判断
        if (blockList.Count > 0 && blockList != null)
        {
            for(int i = 0; i < blockList.Count; i++)
            {
                blockList[i].Die(); // 销毁触发区域内的物体
            }
        }
        OnClear(); // 首尾工作
    }

    /// <summary>
    /// 重写：销毁这只小鸟，实例化下只小鸟
    ///     原方法中还实例化了一次爆炸特效，更此处爆炸时重合了，所以需要删除那一行代码
    /// </summary>
    protected override void Next()
    {
        GameManager.Instance.birdList.Remove(this); // 从集合中移除这只小鸟脚本
        Destroy(gameObject); // 销毁这只小鸟
        GameManager.Instance.IsGameOver(); // 判断游戏是否结束
    }

    /// <summary>
    /// 爆炸后的首收尾工作
    /// </summary>
    private void OnClear()
    {
        rg.velocity = Vector3.zero; // 速度归零
        Instantiate(boom, transform.position, Quaternion.identity); // 实例化爆炸特效
        render.enabled = false; // 禁用图片渲染
        GetComponent<CircleCollider2D>().enabled = false; // 禁用碰撞器
        birdTrail.ClearTrail(); // 清除拖尾
    }

    /// <summary>
    /// 进入触发区域
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            blockList.Add(collision.gameObject.GetComponent<Pig>());
        }
    }

    /// <summary>
    /// 离开触发区域
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            blockList.Remove(collision.gameObject.GetComponent<Pig>());
        }
    }
}
