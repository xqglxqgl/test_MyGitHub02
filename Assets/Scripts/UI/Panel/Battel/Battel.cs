using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Battel : MonoBehaviour
{
    [SerializeField] private Transform hpBar;
    [SerializeField] private Transform hpBarFill;

    [SerializeField] private Image avatar;

    private float currentHp;
    private float maxHp;
    private bool isLow;


    private float waitForSeconds = 1f;
    private float lastFlashTime;

    void Awake()
    {
        lastFlashTime = Time.time;
    }
    void Start()
    {
        PlayerManager.instance.onHpChanged += UpdateHp;
        PlayerManager.instance.onHpIsLow += OnPlayerHpLow;
    }

    void Update()
    {
        FlashingAvatar();
        
    }

    private void UpdateHp(float currentHp, float maxHp)
    {
        // 玩家受到伤害时,血条缩放
        hpBar.DOScale(1.1f,0.1f).OnComplete(()=>
        {
            hpBar.DOScale(1f,0.1f);
        });

        // 玩家受到伤害时,血条填充更新
        this.currentHp = currentHp;
        this.maxHp = maxHp;
        hpBarFill.DOScaleX(this.currentHp/this.maxHp,0.1f);

    }

    private void OnPlayerHpLow(bool isLow)
    {
        this.isLow = isLow;
    }




    private void FlashingAvatar()
    {
        if(isLow && Time.time - lastFlashTime >= waitForSeconds)
        {
            avatar.DOColor(Color.red, 0.5f).OnComplete(()=>
            {
                avatar.DOColor(Color.white, 0.5f).OnComplete(()=>
                {
                    lastFlashTime = Time.time;
                });
            });
        }
    }
}
