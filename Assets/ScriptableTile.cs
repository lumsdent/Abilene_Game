using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class ScriptableTile : ScriptableObject
{
    public TileBase[] tiles;

    public bool canDecay;

    public float spreadChance, decayTime;
}
