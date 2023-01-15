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
    public float startTime, spreadInterval;

    [SerializeField]
    public Difficulty difficulty = Difficulty.Hard;


    private List<Vector3Int> activeDecayList = new List<Vector3Int>();

    private List<Vector3Int> decayedTileList = new List<Vector3Int>();

    private void Start()
    {
        SetupDecaySpawn();
        InvokeRepeating(nameof(SpreadFromDecayedTile), startTime, spreadInterval);
    }

    //Test Method; Non-essential for Gameplay
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float changeValue = 3f;
            UpdateSpreadInterval(changeValue);

            /*
                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector3Int gridPosition = map.WorldToCell(mousePosition);

                        ScriptableTile data = mapManager.GetScriptableTileData(gridPosition);
                        InstantiateDecaying(gridPosition, data);*/
        }
    }

    private void SetupDecaySpawn()
    {
        //Pick a number of random spots on the map at beginning of game to start decay based on difficulty
        map.CompressBounds();
        BoundsInt bounds = map.cellBounds;
        int decaySpawnCounter = 0;
        while (decaySpawnCounter < (int)difficulty)
        {
            Vector3Int gridPosition = new Vector3Int(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax));
            // Random spots must be outside protection circle
            if (DecayCircle.IsOutsideCircle((Vector3)gridPosition))
            {
                ScriptableTile data = mapManager.GetScriptableTileData(gridPosition);
                InstantiateDecaying(gridPosition, data);
                decaySpawnCounter++;
            }
        }
    }

    private void InstantiateDecaying(Vector3Int vector3Int, ScriptableTile data)
    {
        ActiveDecay activeDecayTile = Instantiate(activeDecayPrefab);
        activeDecayTile.transform.position = map.GetCellCenterWorld(vector3Int);
        activeDecayTile.StartDecay(vector3Int, data, this);
        activeDecayList.Add(vector3Int);
    }

    void SpreadFromDecayedTile()
    {
        foreach (Vector3Int position in decayedTileList)
        {
            TryToSpread(position);
        }
    }
    public void TryToSpread(Vector3Int position)
    {
        //for each block surrounding decay tile
        for (int x = position.x - 1; x < position.x + 2; x++)
        {
            for (int y = position.y - 1; y < position.y + 2; y++)
            {
                TryToStartDecaying(new Vector3Int(x, y));
            }
        }
    }

    private void TryToStartDecaying(Vector3Int tilePosition)
    {
        if (activeDecayList.Contains(tilePosition)) return;

        ScriptableTile data = mapManager.GetScriptableTileData(tilePosition);
        if(data != null && data.canDecay)
        {
            if(Random.Range(0f,100f) <= data.spreadChance) {
                InstantiateDecaying(tilePosition, data);
            }
        }
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
 
    private void UpdateSpreadInterval(float changeValue)
    {
        spreadInterval += changeValue;
        CancelInvoke(nameof(SpreadFromDecayedTile));
        InvokeRepeating(nameof(SpreadFromDecayedTile), spreadInterval, spreadInterval);
    }

  //Going to need to figure out how to decay decorations, like trees and maybe houses
}

public enum Difficulty
{
    Peaceful = 0,
    Easy = 3,
    Medium = 5,
    Hard = 7
}
