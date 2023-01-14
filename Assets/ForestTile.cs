using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForestTile : Tile
{

    public Sprite aliveTile;
    public Sprite decayedTile;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        if(DecayCircle.IsOutsideCircle(position))
        {
            tileData.sprite = decayedTile;
        }
    }

}
