using System;
using System.Collections;
using System.Collections.Generic;
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


    private List<Vector3Int> activeDecayList = new List<Vector3Int>();
    
    public void TryToSpread(Vector3Int position, float spreadChance)
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
        Debug.Log(data.ToString());
        if(data != null && data.canDecay)
        {
            if(Random.Range(0f,100f) <= data.spreadChance) {
                Debug.Log("Here");
                SetTileDecayed(tilePosition, data);
            }
        }
    }

    private void SetTileDecayed(Vector3Int vector3Int, ScriptableTile data)
    {
        ActiveDecay activeDecayTile = Instantiate(activeDecayPrefab);
        activeDecayTile.transform.position = map.GetCellCenterWorld(vector3Int);
        activeDecayTile.StartDecay(vector3Int, data, this);
        activeDecayList.Add(vector3Int);
    }

    internal void FinishDecay(Vector3Int position)
    {
        map.SetTile(position, decayTile);
        activeDecayList.Remove(position);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            ScriptableTile data = mapManager.GetScriptableTileData(gridPosition);
            SetTileDecayed(gridPosition, data);
        }
    }
}
