using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 地图的选择
/// </summary>
public class MapSelect : MonoBehaviour {
    public int starsNum = 0; // 开启地图所需要的星星数
    private bool isSelect = false; // 是否选择该地图

    public GameObject lockPanel; // 锁模块
    public GameObject startPanel; // 星星模块
    public GameObject levelPanel; // 地图对应的关卡界面
    public GameObject mapPanel; // 地图界面

    public Text txtStartCount; // 解锁地图后，界面显示的已获取星星数

    public int startNum = 1; // 该张地图的开始关卡数
    public int endtNum = 9; // 该张地图的结束关卡数


    private void Start()
    {
        //PlayerPrefs.DeleteAll(); // 清除所有数据

        if (PlayerPrefs.GetInt("totalNum", 0) >= starsNum) // 判断当前拥有的星星数
        {
            isSelect = true; // 选择当前地图
        }

        if (isSelect)
        {
            lockPanel.SetActive(false); // 禁用 Lock 面板
            startPanel.SetActive(true); // 开启 星星 面板

            int getCount = 0; // 该张地图已获得的星星数
            int count = (endtNum - startNum + 1) * 3; // 该张地图能够获得的星星总数
            for (int i = startNum; i < endtNum; i++)
            {
                getCount += PlayerPrefs.GetInt("level" + i.ToString());
            }
            txtStartCount.text = getCount.ToString() + "/" + count; // 赋值给 UI 界面
        }
    }

    /// <summary>
    /// 鼠标点击事件,跳转到地图对应的关卡界面
    /// </summary>
    public void Select()
    {
        if (isSelect)
        {
            levelPanel.SetActive(true); // 激活关卡界面
            mapPanel.SetActive(false); // 禁用地图界面
        }
    }

    /// <summary>
    /// 返回地图选择界面
    /// </summary>
    public void Back()
    {
        levelPanel.SetActive(false); // 激活地图界面
        mapPanel.SetActive(true); // 禁用关卡界面
    }
}
