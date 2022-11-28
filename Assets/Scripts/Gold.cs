using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] private int _gold;
    [HideInInspector] public TextParse textParse;
    void Start()
    {
        textParse = GetComponent<TextParse>();
        SetGold(0);
    }
    public void SetGold(int value)
    {
        _gold += value;
        textParse.Check(_gold);
        textParse.text = GameManager.CaclText(_gold);
    }
    public int GetGold()
    {
        return _gold;
    }
}
