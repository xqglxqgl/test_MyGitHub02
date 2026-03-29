using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PArcher : Player
{
    private AudioSource bowDrawAS;
    private AudioSource bowReleaseAS;

    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector2.zero;

        this.animationHandler = this.view.GetComponent<AnimationHandler>();
        // 初始化动画事件
        this.animationHandler.onBowDraw += BowDraw;
        this.animationHandler.onBowRelease += BowRelease;
    }

    public override void OnDie()
    {
        base.OnDie();
        this.animationHandler.onBowDraw -= BowDraw;
        this.animationHandler.onBowRelease -= BowRelease;
        Pool.Instance.Recycle(this.view);
    }

    private void BowRelease()
    {
        var prefab = AssetPathUtility.ItemView_ArrowP;
        var offset = new Vector2(0f, -0.16f);
        var position = (Vector2)this.transform.position + offset;
        var dir = this.attackDir.normalized;
        // 生成箭矢
        var arrow = (Arrow)ItemManager.Instance.SpawnArrow(prefab);
        PlayBowReleaseAudio();//播放释放弓的音效
        // 初始化箭矢属性
        arrow.owner = this;
        arrow.TargetLayer = LayerMask.GetMask("MonsterView");
        arrow.Speed = this.property.arrowSpeed;
        arrow.MaxFlyDistance = this.property.arrowMaxFlyDistance;
        arrow.PierceCount = this.property.pierceCount;
        arrow.Damage = this.property.damage;
        arrow.Dir = dir;
        arrow.OutShootPos = position;
        arrow.transform.position = position;
        arrow.transform.right = dir;
    }
    private void BowDraw()
    {
        PlayBowDrawAudio();
    }

    private void PlayBowDrawAudio()//播放拉弓的音效,但是不主动停止音效的播放
    {
        bowDrawAS = AudioManager.Instance.CreateAudioSource(AssetPathUtility.ASPrefab_CombatSFX, AssetPathUtility.AC_Fight_BowDraw);
        bowDrawAS.transform.position = this.transform.position;
        bowDrawAS.volume = 0.2f;
        bowDrawAS.Play();
    }
    private void PlayBowReleaseAudio()//播放释放弓的音效
    {
        // 先回收拉弓的音效
        if (bowDrawAS != null) Pool.Instance.Recycle(bowDrawAS.gameObject);

        // 再播放释放弓的音效
        bowReleaseAS = AudioManager.Instance.CreateAudioSource(AssetPathUtility.ASPrefab_CombatSFX, AssetPathUtility.AC_Fight_BowRelease);
        bowReleaseAS.transform.position = this.transform.position;
        bowReleaseAS.volume = 0.2f;
        bowReleaseAS.Play();
        Pool.Instance.Recycle(bowReleaseAS.gameObject, bowReleaseAS.clip.length);
    }


    void Update()
    {
        JudgeFlip();
        IsRunOrIdle();
        AutoLockTarget();

        JudgeAttack();
    }
    void FixedUpdate()
    {
        UpdateMovment();
    }

}
