using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class RoomNameInputField : Photon.PunBehaviour {

    public string roomName = null;

    public void SetRoomName()
    {

        string value = GetComponent<InputField>().text;
        bool nameIsUnique = true;

        if (PhotonNetwork.insideLobby)
        {
            // Check there isnt an existing room with this name
            RoomInfo[] roomList = PhotonNetwork.GetRoomList();
            foreach (RoomInfo room in roomList)
            {
                if(room.Name == value)
                {
                    nameIsUnique = false;
                }
            }
        }

        if (nameIsUnique)
        {
            roomName = value;
        }
    }
}
