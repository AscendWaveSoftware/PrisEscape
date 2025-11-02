using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetBootstrap : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameVersion = "0.1";
    [SerializeField] private string roomName = "BreakThePrison";
    [SerializeField] private byte maxPlayer = 5;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon Cloud...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");

        PhotonNetwork.JoinOrCreateRoom(
            roomName,
            new RoomOptions { MaxPlayers = maxPlayer },
            TypedLobby.Default
            );
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + roomName);

        Vector3 spawnPos = new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f));
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Disconnected from Photon " + cause);
    }
}
