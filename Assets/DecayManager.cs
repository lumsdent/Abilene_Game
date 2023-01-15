using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DecayManager : MonoBehaviour
{

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private TileBase decayTile;


    [SerializeField]
    private MapManager mapManager;

    [SerializeField]
    private ActiveDecay activeDecayPrefab;
    
    [SerializeField]
    public float spreadInterval;


    private List<Vector3Int> activeDecayList = new List<Vector3Int>();

    private List<Vector3Int> decayedTileList = new List<Vector3Int>();

    public void TryToSpread(Vector3Int position)
    {
        for (int x = position.x - 1; x < position.x + 2; x++)
        {
            for (int y = position.y - 1; y < position.y + 2; y++)
            {
                TryToDecayTile(new Vector3Int(x, y));
            }
        }
    }

    private void TryToDecayTile(Vector3Int tilePosition)
    {
        if (activeDecayList.Contains(tilePosition)) return;

        ScriptableTile data = mapManager.GetScriptableTileData(tilePosition);
        if(data != null && data.canDecay)
        {
            if(Random.Range(0f,100f) <= data.spreadChance) {
                SetTileDecaying(tilePosition, data);
            }
        }
    }

    private void SetTileDecaying(Vector3Int vector3Int, ScriptableTile data)
    {
        ActiveDecay activeDecayTile = Instantiate(activeDecayPrefab);
        activeDecayTile.transform.position = map.GetCellCenterWorld(vector3Int);
        activeDecayTile.StartDecay(vector3Int, data, this);
        activeDecayList.Add(vector3Int);
    }

    internal void FinishDecaying(Vector3Int position)
    {
        SetTileDecayed(position);
        activeDecayList.Remove(position);
    }

    private void SetTileDecayed(Vector3Int position)
    {
        map.SetTile(position, decayTile);
        decayedTileList.Add(position);
    }
 
    private void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            ScriptableTile data = mapManager.GetScriptableTileData(gridPosition);
            SetTileDecaying(gridPosition, data);
        }
    }

    void TryToDecayTile()
    {
        foreach(Vector3Int position in decayedTileList)
        {
            TryToSpread(position);
        }
    }


    //Pick random spots on the map at beginning of game to start decay
    private void Start()
    {

        InvokeRepeating(nameof(TryToDecayTile), 5f, spreadInterval);
        
        BoundsInt bounds = map.cellBounds;
        Vector3Int gridPosition = new Vector3Int(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax));
        ScriptableTile data = mapManager.GetScriptableTileData(gridPosition);
        SetTileDecaying(gridPosition, data);

    }


    //Right now, spread interval can not be > decay interval because it will destroy active decay object.  Move that

    //Going to need to figure out how to decay decorations, like trees and maybe houses
}
