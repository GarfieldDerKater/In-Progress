using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerHand;
    public GameObject OpponentHand;
    public GameObject Table;

    List<GameObject> cards = new List<GameObject>();

     public override void OnStartClient()
     {
         base.OnStartClient();

         PlayerHand = GameObject.Find("PlayerHand");
         OpponentHand = GameObject.Find("OpponentHand");
         Table = GameObject.Find("Table");
     }

     [Server]
     public override void OnStartServer()
     {
         base.OnStartServer();
         cards.Add(Card1);
         //cards.Add(Card2);
     }

     public void OnClick()
    {
        for (int i= 0; i < 5;i++)
        {
            GameObject card = Instantiate(Card1, new Vector2(0, 0), Quaternion.identity);
            card.transform.SetParent(PlayerHand.transform,false);
        }
    
    }
}
