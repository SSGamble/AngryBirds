using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    public bool isSelect = false;
    //private Image image;
    //public Sprite levelBG; // 关卡背景图
    public GameObject[] starts; // 该关卡获得的星星的数量

    private void Awake()
    {
        //image = GetComponent<Image>();
    }

    private void Start()
    {
        if (transform.parent.GetChild(0).name == gameObject.name) // 第一个关卡
        {
            isSelect = true;
        }
        else // 判断当前关卡是否可以选择
        {
            int beforeNum = int.Parse(gameObject.name) - 1; // 前一个关卡数
            if(PlayerPrefs.GetInt("level" + beforeNum.ToString()) > 0) // 如果关卡的星星数 > 0
            {
                isSelect = true; //开启关卡
            }
        }

        if (isSelect)
        {
            //image.overrideSprite = levelBG;
            transform.Find("num").gameObject.SetActive(true); // 激活 num

            // 获取现在关卡对应的名字，然后获得对应的星星个数
            int count = PlayerPrefs.GetInt("level" + gameObject.name); 
            if (count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    starts[i].SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// 选择了这个关卡
    /// </summary>
    public void Select()
    {
        if (isSelect) // 将该关卡的名字存下来
        {
            PlayerPrefs.SetString("nowLevel", "level" + gameObject.name);
            SceneManager.LoadScene(2);
        }
    }
}
