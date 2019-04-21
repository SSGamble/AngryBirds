using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 小鸟的个数其实就是，小鸟脚本的个数，猪同理
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // 单例

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    public List<Bird> birdList; // 小鸟脚本集合
    public List<Pig> pigList; // 猪脚本集合

    private Vector3 origionPos; // 小鸟的初始化位置

    public GameObject win; // 赢的动画
    public GameObject lose; // 输的动画

    public GameObject[] starts; // 星星数组 

    private int startNum = 0; // 星星数量
    private int totalNum = 10; // 关卡数

    private void Awake()
    {
        Instance = this;
        if (birdList.Count > 0)
        {
            origionPos = birdList[0].transform.position; // 将第一只小鸟放到初始位置
        }
    }

    private void Start()
    {
        InitBirds();
    }

    /// <summary>
    /// 初始化小鸟们，遍历小鸟集合，处理小鸟集合中小鸟的状态
    /// </summary>
    private void InitBirds()
    {
        for (int i = 0; i < birdList.Count; i++)
        {
            if (i == 0) // 第一只小鸟
            {
                birdList[i].transform.position = origionPos; // 将集合中第一只小鸟放到初始位置
                birdList[i].enabled = true; // 启用 Bird 脚本
                birdList[i].sj.enabled = true; // 启用弹簧关节
                birdList[i].canMove = true; // 可以移动第一只小鸟了
            }
            else // 不是第一只小鸟，禁用两个组件
            {
                birdList[i].enabled = false;
                birdList[i].sj.enabled = false;
                birdList[i].canMove = false; 
            }
        }
    }

    /// <summary>
    /// 判断游戏逻辑
    /// </summary>
    public void IsGameOver()
    {
        if (pigList.Count > 0) // 场上还有猪
        {
            if (birdList.Count > 0) // 还有没有鸟
            {
                InitBirds();
            }
            else // 输了
            {
                lose.SetActive(true); // 激活输的动画
            }
        }
        else // 赢了
        {
            win.SetActive(true); // 激活赢的动画
        }
    }

    /// <summary>
    /// 显示星星
    /// </summary>
    public void ShowStart()
    {
        StartCoroutine("FadeShowStart");
    }

    /// <summary>
    /// 逐渐显示星星
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeShowStart()
    {
        // 游戏结束时，剩 n 只小鸟，就出现 n + 1 颗星星
        for(; startNum < birdList.Count + 1; startNum++)
        {
            if (startNum >= starts.Length) // 场上可能不止 3 只小鸟，防止越界
            {
                break;
            }
            yield return new WaitForSeconds(0.3f);
            starts[startNum].SetActive(true);
        }
    }

    /// <summary>
    /// 重玩本关
    /// </summary>
    public void Restart()
    {
        SaveData();
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// 返回 Home 界面
    /// </summary>
    public void Home()
    {
        SaveData();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        // 如果当前星星数 > 历史记录，就存储下来
        if (startNum > PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel")))
        {
            PlayerPrefs.SetInt(PlayerPrefs.GetString("nowLevel"),startNum);
        }

        int sum = 0; // 星星总数
        for(int i = 1; i <= totalNum; i++)
        {
            sum += PlayerPrefs.GetInt("level" + i.ToString());
        }
        PlayerPrefs.SetInt("totalNum", sum); // 存储星星总数
    }
}
