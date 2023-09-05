using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that details the behavior of a teleporter object
/// </summary>
public class Teleporter : MonoBehaviour
{
    public Teleporter otherTeleporter;
    public Action<Vector3> OnTeleport;
    public AudioSource teleportSound;
    public ParticleSystem teleporterFX;

    private bool teleporterOn = true;
    private Movement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerMovement == null)
                playerMovement = other.GetComponent<Movement>();
            //Only the movement script should control positioning 
            OnTeleport -= playerMovement.ForceMove;
            OnTeleport += playerMovement.ForceMove;

            //we only want to teleport if there is somewhere to teleport to
            if (otherTeleporter != null && this.teleporterOn)
            {
                //this it to prevent immeadiately teleporting back 
                /// TODO: this can be re-written so that instead of using complicated 
                /// on/off strategies, the player is simply teleported a little bit
                /// farther than the exact positioning of the other teleporter, so
                /// that they miss triggering the "otherTeleporter" trigger
                this.teleporterOn = false;
                otherTeleporter.teleporterOn = false;
                
                //play sound
                teleportSound.Play();

                //play FX
                teleporterFX.Play();
                otherTeleporter.teleporterFX.Play();

                OnTeleport?.Invoke(otherTeleporter.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerMovement == null)
                playerMovement = other.GetComponent<Movement>();
            OnTeleport -= playerMovement.ForceMove;

            //setting both teleporters back to "on"
            //the "from" teleporter is also triggered with this method as soon as the 
            //teleport actually happens
            if (otherTeleporter != null && !this.teleporterOn)
            {
                this.teleporterOn = true;
            }
        }
    }

    //just in case! don't want a memory leak
    private void OnDestroy()
    {
        OnTeleport -= playerMovement.ForceMove;
    }
}
