using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that takes the input to drop a teleporter
/// </summary>
public class PlaceTeleporters : MonoBehaviour
{
    public Action<PlaceTeleporters, Vector3> OnTeleporterPlaced;

    void Start()
    {
        TeleporterManager.Instance.AddTeleportingPlayer(this);
    }

    void Update()
    {
        if (Input.GetButtonDown("PlaceTeleporter"))
        {
            OnTeleporterPlaced?.Invoke(this, transform.position + (transform.forward * 1.5f));
        }
    }
}
