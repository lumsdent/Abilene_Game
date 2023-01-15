using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ActiveDecay : MonoBehaviour
{
    Vector3Int position;
    ScriptableTile tileData;
    DecayManager decayManager;
    private float timer, waitTimer;

    internal void StartDecay(Vector3Int vector3Int, ScriptableTile data, DecayManager decayManager)
    {
        this.position = vector3Int;
        this.tileData = data;
        this.decayManager = decayManager;

        timer = data.decayTime;
        waitTimer = data.spreadInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Debug.Log("WE MADE IT");
            decayManager.FinishDecay(position);
            Destroy(gameObject);
        }
        waitTimer -= Time.deltaTime;
        if(waitTimer <= 0)
        {
            waitTimer = tileData.spreadInterval;
            decayManager.TryToSpread(position, tileData.spreadChance);
        }
    }
}
