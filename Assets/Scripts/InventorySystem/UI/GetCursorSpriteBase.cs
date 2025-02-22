using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCursorSpriteBase : ScriptableObject
{
    [SerializeField] protected Sprite _default;
    public virtual Sprite GetSprite()
    {
        return _default;
    }
}
