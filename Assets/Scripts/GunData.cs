using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Gun")]
public class GunData : ScriptableObject
{
    [SerializeField] private float _boostedAttackRate;
    [SerializeField] private float _attackRate; 
    [SerializeField] private int _earnedMoneyPerFire; 
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Gun _nextGun; // birleþtikten sonra oluscak sýlah

    public float AttackRate => _attackRate;
    public float BoostedAttackRate => _boostedAttackRate;
    public int EarndMoneyPerFire => _earnedMoneyPerFire;
    public float MoveSpeed => _moveSpeed;
    public Gun NextGun => _nextGun;

}
