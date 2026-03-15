using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Battel : MonoBehaviour
{
    [SerializeField] private Transform hpBar;
    [SerializeField] private Transform hpBarFill;

    [SerializeField] private Transform avatar;

    void Start()
    {
        //玩家血条填充更新
        hpBarFill.DOScaleX(GameManager.Instance.player.currentHealth/GameManager.Instance.player.maxHealth,0.1f);
        //订阅玩家受到 伤害事件
        GameManager.Instance.onPlayerTakeDamage += OnPlayerTakeDamage;
        //订阅玩家 低血量事件
        GameManager.Instance.onPlayerHpLow += OnPlayerHpLow;
    }

    private void OnPlayerTakeDamage()
    {
        // 玩家受到伤害时,血条缩放
        hpBar.DOScale(1.1f,0.1f).OnComplete(()=>
        {
            hpBar.DOScale(1f,0.1f);
        });

        // 玩家受到伤害时,血条填充更新
        hpBarFill.DOScaleX(GameManager.Instance.player.currentHealth/GameManager.Instance.player.maxHealth,0.1f);

    }

    private void OnPlayerHpLow()
    {
        // 玩家血量低时,开始闪烁
        StartCoroutine(FlashAvatar());
    }
    private IEnumerator FlashAvatar()
    {
        Image sprite = avatar.GetComponent<Image>();
        float waitForSeconds = 1f;
        while (GameManager.Instance.player.currentHealth/GameManager.Instance.player.maxHealth < 0.3f)  // 协程中的while(true)是安全的
        {
            
            sprite.DOColor(Color.red, 0.5f).OnComplete(()=>
            {
                sprite.DOColor(Color.white, 0.5f);
            });
            yield return new WaitForSeconds(waitForSeconds);  // 等待1秒
        }
    }
}
