using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    public Image healthBar;

    public Image mpBar;

    public Image FlawImage;

    public float health;

    public float mp;

    /// <summary>
    /// 基础的二维变化
    /// </summary>
    /// <param name="_health"></param>
    /// <param name="_mp"></param>
    public void StateChanged(float _healthFill, float _mpFill)
    {
        healthBar.fillAmount = Mathf.Min(1, _healthFill);

        mpBar.fillAmount = Mathf.Min(1, _mpFill);
    }

    /// <summary>
    /// 破绽状态受到改变（获得或失去）
    /// </summary>
    /// <param name="_delta"></param>
    public void FlawChanged(bool _delta)
    {
        FlawImage.sprite
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
