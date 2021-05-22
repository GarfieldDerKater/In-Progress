using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class DragDrop : NetworkBehaviour
{
    private bool isDragging = false;
    private bool isDraggable = false;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject table;
    private bool isOverTable;
    
    
    
    

    public GameObject Canvas;
    public PlayerManager PlayerManager;
    public GameObject PlayerHand;
    ////public GameObject Table;
    
    void Start()
    {
        Canvas = GameObject.Find("GameCanvas");
        PlayerHand = GameObject.Find("PlayerHand");
        ////Table = GameObject.Find("Table");
        if(hasAuthority)
        {
            isDraggable = true;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        isOverTable = true;
        
        table = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        isOverTable = false;
        
        table = null;
    }

    public void StartDrag()
    {
        if(PlayerHand.transform.childCount!=6) isDraggable = false;
        if(!isDraggable) return;
        
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndDrag()
    {
        if(!isDraggable) return;
         

        isDragging = false;
        if (isOverTable)
        {
            transform.SetParent(table.transform,false);
            isDraggable = false;
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();
            PlayerManager.PlayCard(gameObject);
            
                    
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform,false);
        }
    }
    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
            transform.SetParent(Canvas.transform,true);
        }
    }
}
