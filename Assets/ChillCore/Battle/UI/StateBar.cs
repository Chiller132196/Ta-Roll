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

    public Camera mainCamera;

    /// <summary>
    /// �����Ķ�ά�仯
    /// </summary>
    /// <param name="_health"></param>
    /// <param name="_mp"></param>
    public void StateChanged(float _healthFill, float _mpFill)
    {
        healthBar.fillAmount = Mathf.Min(1, _healthFill);

        mpBar.fillAmount = Mathf.Min(1, _mpFill);
    }

    /// <summary>
    /// ����״̬�ܵ��ı䣨��û�ʧȥ��
    /// </summary>
    /// <param name="_delta"></param>
    public void FlawChanged(bool _delta)
    {
        FlawImage.enabled = _delta;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.forward = mainCamera.transform.forward;
    }
}
