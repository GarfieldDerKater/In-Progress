using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject table;
    private bool isOverTable;
    
    

    public GameObject Canvas;
    public GameObject Table;
    
    void Start()
    {
        Canvas = GameObject.Find("GameCanvas");
        Table = GameObject.Find("Table");
        
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
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverTable)
        {
            transform.SetParent(table.transform,false);
                    
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
