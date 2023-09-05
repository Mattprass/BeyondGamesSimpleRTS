using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public static TeleporterManager Instance { get; private set; }

    [SerializeField]
    private GameObject teleporterPrefab;
    
    private int teleportersPlaced;
    private Dictionary<PlaceTeleporters, PlayerTeleporterInfo> playersWhoCanPlaceTeleporters;

    private const int MAX_TELEPORTERS_PER_PLAYER = 2;

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

    public void AddTeleportingPlayer(PlaceTeleporters player)
    {
        player.OnTeleporterPlaced -= PlaceTeleporterForPlayer;
        player.OnTeleporterPlaced += PlaceTeleporterForPlayer;
        playersWhoCanPlaceTeleporters.Add(player, new PlayerTeleporterInfo());
    }

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

    private IEnumerator DestoryTeleporters(PlayerTeleporterInfo teleporters)
    {
        yield return new WaitForSeconds(10f);

        Destroy(teleporters.teleporterOne.gameObject);
        Destroy(teleporters.teleporterTwo.gameObject);
        teleportersPlaced = 0;

        Debug.Log("You can now place more teleporters!");
    }

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
