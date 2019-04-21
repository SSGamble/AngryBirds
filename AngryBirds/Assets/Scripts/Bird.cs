using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour
{
    protected Rigidbody2D rg; // 刚体
    protected SpriteRenderer render;  //精灵渲染器
    [HideInInspector] // 虽然是 public 变量但在面板里看不见
    public SpringJoint2D sj; // 弹簧关节

    public LineRenderer lrLeft; // 左边树枝的弹簧点坐标
    public Transform leftPos; // 左边的 LineRender
    public LineRenderer lrRight; // 右边的 LineRender
    public Transform rightPos; // 右边树枝的弹簧点坐标
    
    private bool isClick = false; // 是否按下鼠标
    private bool isFly = false; // 是否处于飞行状态
    [HideInInspector]
    public bool isMouseRelease; // 是否松开鼠标
    [HideInInspector]
    public bool canMove = false; // 小鸟是否能移动
    public float maxDis = 3; // 允许弹簧弹力的最远距离
    public float smooth = 3; // 相机跟随

    public GameObject boom; // 爆炸特效
    protected BirdTrail birdTrail; // 拖尾脚本
    public Sprite hurt; // 小鸟受伤

    public AudioClip select;
    public AudioClip fly;


    private void Awake()
    {
        sj = GetComponent<SpringJoint2D>(); // 获取弹簧关节
        rg = GetComponent<Rigidbody2D>(); // 获取刚体组件
        birdTrail = GetComponent<BirdTrail>(); // 获取拖尾脚本
        render = GetComponent<SpriteRenderer>(); // 获取精灵渲染器

    }

    private void Update()
    {
        // 防止点击暂停按钮触发技能小鸟的技能
        if (EventSystem.current.IsPointerOverGameObject()) // 是否点击 UI 界面
        {
            return;
        }

        // 鼠标按下，小鸟位置跟随鼠标
        if (isClick)
        {
            // public Vector3 ScreenToWorldPoint(Vector3 position); 将位置从屏幕空间转换为世界空间。
            // 屏幕空间以像素为单位定义。屏幕左下角为(0,0);右上角是(像素宽度，像素高度)。z的位置是以摄像机的世界单位为单位的。
            // 所以，这里转换换后会有一个 z 坐标，导致不在当前平面内
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 减去主摄像机的 z 坐标，即：-10-(-10) = 0，将 z 轴归为 0
            transform.position -= new Vector3(0, 0, Camera.main.transform.position.z);

            // 距离限定
            if (Vector3.Distance(transform.position, rightPos.position) > maxDis)
            {
                // Vector3 Vector3.normalized;  返回大小为 1的向量(只读)。
                Vector3 pos = (transform.position - rightPos.position).normalized; // 单位化向量
                pos *= maxDis; // 所允许的最大长度的向量
                transform.position = pos + rightPos.position; // 将允许的最大位置赋给小鸟
            }

            DrawLine(); // 画皮筋
        }

        CameraFollowBird(); // 相机跟随
        ShowSkillMouseEvent(); // 释放技能的鼠标事件触发

    }

    /// <summary>
    /// 释放技能的鼠标事件触发
    /// </summary>
    private void ShowSkillMouseEvent()
    {
        if (isFly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShowSkill();
            }
        }
    }

    /// <summary>
    /// 施展特殊技能，虚方法，供给子类重写
    /// </summary>
    public virtual void ShowSkill()
    {
        isFly = false;
    }

    /// <summary>
    /// 相机跟随，让相机在一定范围内跟随小鸟
    /// </summary>
    private void CameraFollowBird()
    {
        float posX = transform.position.x; // 当前小鸟的位置
        // public static Vector3 Lerp(Vector3 a, Vector3 b, float t); 在 a,b 间，插值
        Camera.main.transform.position = Vector3.Lerp( // 设置相机的位置，插值
            Camera.main.transform.position, // 当前位置
                                            // Mathf.Clamp: 夹紧，将 posX 的值限定在 (0,30) 范围内，超过 30 就不用跟随了
            new Vector3(Mathf.Clamp(posX, 0, 30), Camera.main.transform.position.y, Camera.main.transform.position.z), // 目标位置
            smooth * Time.deltaTime);
    }

    /// <summary>
    /// 鼠标按下
    /// </summary>
    private void OnMouseDown()
    {
        if (canMove)
        {
            AudioPlay(select);
            isClick = true;
            rg.isKinematic = true; // 开启动力学
        }
    }

    /// <summary>
    /// 鼠标抬起
    /// </summary>
    private void OnMouseUp()
    {
        if (canMove)
        {
            isClick = false;
            rg.isKinematic = false; // 关闭动力学
            Invoke("Fly", 0.1f); // 不能马上禁用弹簧关节，应该给他一个计算的时间，所以要延时禁用弹簧关节
            // 禁用划线组件
            lrLeft.enabled = false;
            lrRight.enabled = false;

            canMove = false;
        }
    }

    /// <summary>
    /// 飞行
    /// </summary>
    private void Fly()
    {
        isMouseRelease = true;
        isFly = true;
        AudioPlay(fly);
        birdTrail.StartTrail(); // 开启拖尾效果
        sj.enabled = false; // 禁用弹簧关节，实现飞出的效果
        Invoke("Next", 5f); // 1s 后，处理下一只小鸟
    }

    /// <summary>
    /// 划线，皮筋
    /// </summary>
    private void DrawLine()
    {
        // 启用划线组件
        lrLeft.enabled = true;
        lrRight.enabled = true;

        // void LineRenderer.SetPosition(int index, Vector3 position) 设置线中顶点的位置
        // 左边的线
        lrLeft.SetPosition(0, leftPos.position); // 第一个点
        lrLeft.SetPosition(1, transform.position); // 第二个点
        // 右边的线
        lrRight.SetPosition(0, rightPos.position); // 第一个点
        lrRight.SetPosition(1, transform.position); // 第二个点
    }

    /// <summary>
    /// 销毁这只小鸟，实例化下只小鸟
    /// </summary>
    protected virtual void Next()
    {
        GameManager.Instance.birdList.Remove(this); // 从集合中移除这只小鸟脚本
        Destroy(gameObject); // 销毁这只小鸟
        Instantiate(boom, transform.position, Quaternion.identity); // 实例化爆炸特效
        GameManager.Instance.IsGameOver(); // 判断游戏是否结束
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isFly = false;
        birdTrail.ClearTrail(); // 关闭拖尾效果
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public void Hurt()
    {
        render.sprite = hurt; // 改为受伤的图片
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clicp"></param>
    public void AudioPlay(AudioClip clicp)
    {
        AudioSource.PlayClipAtPoint(clicp,transform.position);
    }
}
