using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Teleporter otherTeleporter;

    private bool teleporterOn = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (otherTeleporter != null && this.teleporterOn)
            {
                this.teleporterOn = false;
                otherTeleporter.teleporterOn = false;
                
                other.transform.position = otherTeleporter.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (otherTeleporter != null && !this.teleporterOn)
            {
                this.teleporterOn = true;
            }
        }
    }
}
