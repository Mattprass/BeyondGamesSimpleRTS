using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that manages the teleporting behaviors potentially from multiple players
/// and enforces any rules associated with teleportation.
/// This is intentionally modeled as a "pesudo-server" where similar to a multiplayer 
/// game, the player will need to get permission to place down a teleporter.
/// </summary>
public class TeleporterManager : MonoBehaviour
{
    public static TeleporterManager Instance { get; private set; }

    [SerializeField]
    private GameObject teleporterPrefab;
    
    private int teleportersPlaced;
    //Data structure to hold the teleporting data for each teleporting player
    private Dictionary<PlaceTeleporters, PlayerTeleporterInfo> playersWhoCanPlaceTeleporters;

    //At the moment, players are limited to having 2 teleporters (which forms 1 link)
    private const int MAX_TELEPORTERS_PER_PLAYER = 2;

    //Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        playersWhoCanPlaceTeleporters = new Dictionary<PlaceTeleporters, PlayerTeleporterInfo>();
    }

    //Called at the player's Start() method
    public void AddTeleportingPlayer(PlaceTeleporters player)
    {
        player.OnTeleporterPlaced -= PlaceTeleporterForPlayer;
        player.OnTeleporterPlaced += PlaceTeleporterForPlayer;
        playersWhoCanPlaceTeleporters.Add(player, new PlayerTeleporterInfo());
    }

    //This is what actually places down a teleporter in the world
    ///TODO: Not a big fan of calling the same coniditon twice, but the variable does change
    ///in the middle of the method
    private void PlaceTeleporterForPlayer(PlaceTeleporters player, Vector3 teleporterPosition)
    {
        if (teleportersPlaced == MAX_TELEPORTERS_PER_PLAYER)
        {
            Debug.Log($"Cannot spawn more than {MAX_TELEPORTERS_PER_PLAYER} teleporters per player");
            return;
        }

        playersWhoCanPlaceTeleporters[player].AddTeleporter(
            Instantiate(teleporterPrefab, teleporterPosition, Quaternion.identity)
            .GetComponent<Teleporter>());
        teleportersPlaced++;

        if (teleportersPlaced == MAX_TELEPORTERS_PER_PLAYER)
        {
            StartCoroutine(DestoryTeleporters(playersWhoCanPlaceTeleporters[player]));
        }
    }

    //Destory both teleporters after 10 seconds of BOTH teleporters being spawned
    private IEnumerator DestoryTeleporters(PlayerTeleporterInfo teleporters)
    {
        yield return new WaitForSeconds(10f);

        Destroy(teleporters.teleporterOne.gameObject);
        Destroy(teleporters.teleporterTwo.gameObject);
        teleportersPlaced = 0;

        Debug.Log("You can now place more teleporters!");
    }

    //preventing memory leaks
    private void OnDestroy()
    {
        foreach (var player in playersWhoCanPlaceTeleporters.Keys)
        {
            player.OnTeleporterPlaced -= PlaceTeleporterForPlayer;
        }
    }
}

public class PlayerTeleporterInfo
{
    public Teleporter teleporterOne;
    public Teleporter teleporterTwo;

    //If it's the first teleporter placed for a player, then it becomes teleporterOne
    //If it's the second teleporter placed, then it becomes the teleporterTwo and forms 
    //the link between the first and second (and vice versa)
    public void AddTeleporter(Teleporter tele)
    {
        if (teleporterOne == null)
            teleporterOne = tele;
        else
        {
            teleporterTwo = tele;
            teleporterTwo.otherTeleporter = teleporterOne;
            teleporterOne.otherTeleporter = teleporterTwo;
        }
    }
}
