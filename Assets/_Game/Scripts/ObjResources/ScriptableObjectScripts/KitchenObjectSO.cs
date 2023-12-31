using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/KitchenObject",fileName = "KitchenObject")]
public class KitchenObjectSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public string objectName;
}
