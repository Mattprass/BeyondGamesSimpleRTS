using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
