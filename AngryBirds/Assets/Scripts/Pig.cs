using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 猪和树木的共用脚本
/// </summary>
public class Pig : MonoBehaviour
{
    public float maxSpeed = 10; // 碰撞检测中，超过该速度，判定猪死亡
    public float minSpeed = 5; // 碰撞检测中，至少要超过该速度，判定猪受伤
    private SpriteRenderer render; // 精灵渲染器
    public Sprite hurt; // 猪，受伤的图片
    public GameObject boom; // boom 动画
    public GameObject score; // 打死猪获得的分数物体

    public bool isPig = false; // 是不是猪

    public AudioClip hurtClip;
    public AudioClip dead;
    public AudioClip birdCollision;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            AudioPlay(birdCollision);
            other.transform.GetComponent<Bird>().Hurt(); // 小鸟受伤
        }
        // Vector2 Collision2D.relativeVelocity // 两个碰撞体的相对线速度
        // float Vector2.magnitude // 返回此向量的长度(只读)。
        if (other.relativeVelocity.magnitude > maxSpeed) // 碰撞超过允许的最大速度，猪，死亡
        {
            Die(); // 死亡
        }
        else if (other.relativeVelocity.magnitude < maxSpeed && other.relativeVelocity.magnitude > minSpeed) // 受伤
        {
            render.sprite = hurt; // 改为受伤的图片
            AudioPlay(hurtClip);
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Die()
    {
        if (isPig)
        {
            GameManager.Instance.pigList.Remove(this); // 从集合中移除该脚本
        }
        Destroy(gameObject); // 销毁猪
        Instantiate(boom, transform.position, Quaternion.identity); // 实例化爆炸特效
        GameObject go = Instantiate(score, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity); // 实例化分数
        Destroy(go, 1f); // 1.5s 后销毁分数
        AudioPlay(dead); // 播放音乐
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clicp"></param>
    public void AudioPlay(AudioClip clicp)
    {
        AudioSource.PlayClipAtPoint(clicp, transform.position);
    }
}
