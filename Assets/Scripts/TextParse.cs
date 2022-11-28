using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextParse : TextMeshProUGUI
{
    Color _color = new Color(120, 111, 111,255);
    public override Color color { get => base.color; set => base.color = value; }
    public void Check(int value)
    {
        if (GameManager.Instance.Gold < value)
            color = _color;
        else
        {
            color = Color.black ;
        }
    }
}
