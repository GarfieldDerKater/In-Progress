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
         //cards.Add(Card3);
         //cards.Add(Card4);
         //cards.Add(Card5);
         //cards.Add(Card6);
         //cards.Add(Card7);

     }
     [Command]
     public void CmdDealCards()
    {
        
        if((Table.transform.childCount!=0)&&hasAuthority)
        {
            for (int i=0;i<Table.transform.childCount;i++)
            {
                Destroy(Table.transform.GetChild(0).gameObject);
            }
        }
        else if((Table.transform.childCount!=0)&&(!hasAuthority))
        {
            for (int i=0;i<Table.transform.childCount;i++)
            {
                Destroy(Table.transform.GetChild(0).gameObject);
            }
        }
        if(hasAuthority)
        {
        for (int i= 0; i < (6-PlayerHand.transform.childCount);i++)
        {
            GameObject card = Instantiate(cards[Random.Range(0,cards.Count)], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card,connectionToClient);
            RpcShowCard(card,"Dealt");
            ////card.transform.SetParent(PlayerHand.transform,false);
        }
        }
        else 
        {
            for (int i= 0; i < (6-OpponentHand.transform.childCount);i++)
        {
            GameObject card = Instantiate(cards[Random.Range(0,cards.Count)], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card,connectionToClient);
            RpcShowCard(card,"Dealt");
            ////card.transform.SetParent(PlayerHand.transform,false);
        }
        }
        
       
    
    }

    public void PlayCard(GameObject card)
    {
        
        CmdPlayCard(card);
        
    }
    
    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card,"Played");
        
    }


    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if(type == "Dealt")
        {
            if(hasAuthority)
            {
                card.transform.SetParent(PlayerHand.transform,false);
            }
            else
            {
                card.transform.SetParent(OpponentHand.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }       
        }
        else if(type == "Played")
        {
            card.transform.SetParent(Table.transform,false);
            if(!hasAuthority)
            {
                card.GetComponent<CardFlipper>().Flip();
            }
        }
    }

    [Command]
    public void CmdChooseSelfCard()
    {
        TargetSelfCard();
    }
    [Command]
    public void CmdChooseOpponentCard(GameObject target)
    {
        NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
        TargetOpponentCard(opponentIdentity.connectionToClient);
    }
    [TargetRpc]
    void TargetSelfCard()
    {
        //Debug.Log("Chosen by self");
    }
    [TargetRpc]
    void TargetOpponentCard(NetworkConnection target)
    {
        //Debug.Log("Chosen by opponent");
    }

    [Command]
    public void CmdCountCards(GameObject card)
    {
        RpcCountCards(card);
    }

    [ClientRpc]
    void RpcCountCards(GameObject card)
    {
        card.GetComponent<CountCardsInHand>().numCards = PlayerHand.transform.childCount;
        Debug.Log("Num of cards in hand is "+ card.GetComponent<CountCardsInHand>().numCards);
    }

   
}
