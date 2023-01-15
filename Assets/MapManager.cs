using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private List<ScriptableTile> scriptableTileList;

    private Dictionary<TileBase, ScriptableTile> tileDataDic;

    private void Awake()
    {

        
        tileDataDic= new Dictionary<TileBase, ScriptableTile>();

        foreach (ScriptableTile tileData in scriptableTileList)
        {
            foreach (var tile in tileData.tiles)
            {
                tileDataDic.Add(tile, tileData);
            }
        }
    }

    public ScriptableTile GetScriptableTileData(Vector3Int tilePosition)
    {
        TileBase tile = map.GetTile(tilePosition);
        return tile == null ? null : tileDataDic[tile];
    }



}
