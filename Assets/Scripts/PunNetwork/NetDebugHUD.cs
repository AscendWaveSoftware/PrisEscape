using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetDebugHUD : MonoBehaviourPunCallbacks
{
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 320, 140), GUI.skin.box);
        GUILayout.Label($"Connected: {PhotonNetwork.IsConnected}");
        GUILayout.Label($"State: {PhotonNetwork.NetworkClientState}");
        GUILayout.Label($"Room: {(PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name : "-")}");
        GUILayout.Label($"Players: {(PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.PlayerCount : 0)}");
        GUILayout.EndArea();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"[PUN] Joined: {newPlayer.NickName} ({newPlayer.UserId})");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"[PUN] Left: {otherPlayer.NickName} ({otherPlayer.UserId})");
    }
}
