using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 异步加载场景
/// </summary>
public class LoadLevelAsync : MonoBehaviour {

	void Start () {
        //Screen.SetResolution(960, 600, false); // 设置屏幕分辨率
        Invoke("Load", 2); // 模拟延时
	}

    private void Load()
    {
        SceneManager.LoadSceneAsync(1); // 异步加载场景
    }
}
