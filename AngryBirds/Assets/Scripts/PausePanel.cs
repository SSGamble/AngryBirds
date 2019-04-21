using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour {

    private Animator animator;
    public GameObject pauseButton; // 游戏界面的暂停按钮

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 点击了 Pause 按钮，播放动画
    /// </summary>
    public void Pause()
    {
        animator.SetBool("isPause", true); // 改变动画状态机的 isPause
        pauseButton.SetActive(false); // 隐藏暂停按钮

        // 暂停后，不能操作小鸟
        if (GameManager.Instance.birdList.Count > 0)
        {
            if(GameManager.Instance.birdList[0].isMouseRelease == false) // 小鸟没有飞出
            {
                GameManager.Instance.birdList[0].canMove = false;
            }
        }
    }

    /// <summary>
    /// 点击了 Resume 按钮，播放动画
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1; // 先恢复游戏
        animator.SetBool("isPause", false); // 改变动画状态机的 isPause

        // 恢复后，可以操作小鸟
        if (GameManager.Instance.birdList.Count > 0)
        {
            if (GameManager.Instance.birdList[0].isMouseRelease == false) // 小鸟没有飞出
            {
                GameManager.Instance.birdList[0].canMove = true;
            }
        }
    }

    /// <summary>
    /// 重玩本关
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1; // 先恢复游戏
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// 返回 Home 界面
    /// </summary>
    public void Home()
    {
        Time.timeScale = 1; // 先恢复游戏
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 动画事件，pause 动画播放完调用
    /// </summary>
    public void PauseAnimEnd()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// 动画事件，resume 动画播放完调用
    /// </summary>
    public void ResumeAnimEnd()
    {
        pauseButton.SetActive(true);
    }
}
