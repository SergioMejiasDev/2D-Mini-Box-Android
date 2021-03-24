using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Class that manages the connections with the Photon Server.
/// </summary>
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager networkManager;

    [SerializeField] GameObject panelConnecting = null;
    [SerializeField] GameObject panelRooms = null;
    [SerializeField] GameObject panelErrorRoom = null;

    public Photon.Realtime.Player[] players;
    public int playerNumber = 0;

    public bool isConnected = false;

    public int activeRoom = 0;

    void Awake()
    {
        networkManager = this;
    }

    /// <summary>
    /// Function that resets the player's data on the server.
    /// </summary>
    public void SetValues()
    {
        players = PhotonNetwork.PlayerList;

        if (playerNumber == 1)
        {
            return;
        }

        playerNumber = players.Length;

        PhotonNetwork.NickName = playerNumber.ToString();
    }

    /// <summary>
    /// Function we call to connect to the server.
    /// </summary>
    public void ConnectToServer()
    {
        PhotonNetwork.GameVersion = "0.34";
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = PlayerPrefs.GetString("ActiveRegion", "eu");
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.ConnectUsingSettings();

        panelConnecting.SetActive(true);
    }

    /// <summary>
    /// Function called when the connection to the server is successful.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        isConnected = true;

        panelConnecting.SetActive(false);
        panelRooms.SetActive(true);
    }

    /// <summary>
    /// Function called to disconnect from the server.
    /// </summary>
    public void DisconnectFromServer()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// Function called when you have disconnected from the server.
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        activeRoom = 0;
        isConnected = false;
    }

    /// <summary>
    /// Function called to enter a room.
    /// </summary>
    /// <param name="roomNumber">The number of the room we want to enter.</param>
    public void JoinRoom(int roomNumber)
    {
        activeRoom = roomNumber;
        PhotonNetwork.JoinOrCreateRoom(roomNumber.ToString(), new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    /// <summary>
    /// Function that is called when we have successfully joined a room.
    /// </summary>
    public override void OnJoinedRoom()
    {
        panelRooms.SetActive(false);
    }

    public void CloseRoomError()
    {
        panelErrorRoom.SetActive(false);
        panelRooms.SetActive(true);
    }
}
