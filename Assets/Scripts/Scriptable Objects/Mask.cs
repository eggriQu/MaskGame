using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mask", menuName = "Scriptable Objects/Mask")]
public class Mask : ScriptableObject
{
    public Sprite maskSprite;
    public int maskType;
}
