using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PropertyHandler : MonoBehaviour
{
    [Header("玩家基础属性")]
    [SerializeField]private PlayerProperty playerProperty;
    public PlayerProperty PlayerProperty{ get { return playerProperty; } }

    private float currentHp;
    public float CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;

            onHpChanged?.Invoke();

            if (currentHp <= 0){onHpZero?.Invoke(); return;}


            if (currentHp < playerProperty.maxHp * 0.3)onHpIsLow?.Invoke(true);
            else onHpIsLow?.Invoke(false);
        }
    }

    public UnityAction onHpChanged;
    public UnityAction<bool> onHpIsLow;
    public UnityAction onHpZero;
    void Awake()
    {
        currentHp = playerProperty.maxHp;
    }
    void Start()
    {
        PlayerManager.instance.onTakeDamage += TakeDamage;
    }

    private void TakeDamage(float damage)
    {
        CurrentHp -= damage;
    }
}
