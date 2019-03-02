﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private KeyCode
        upKey = KeyCode.W,
        downKey = KeyCode.S,
        leftKey = KeyCode.A,
        rightKey = KeyCode.D
        
        ;

    [SerializeField] private Rigidbody2D playerRb;

    public Vector3 spawnPosition;
   

    [SerializeField] private float
        defaultSpeed;
    
    [HideInInspector] public float 
        actualSpeed
        ;
      
        
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        actualSpeed = defaultSpeed;
        GameManager.instance.playerMov = this;
        spawnPosition = transform.position;
    }

    void Update()
    {
      
        UpdateMovement();
    }
    void UpdateMovement()
    {
    

        var movement = Vector2.zero;

        if (Input.GetKey(upKey))
        {
            movement.y += 1;
        }
        if (Input.GetKey(downKey))
        {
            movement.y -= 1;

        }
        if (Input.GetKey(rightKey))
        {
            movement.x += 1;
        }
        if (Input.GetKey(leftKey))
        {
            movement.x -= 1;
        }

       

      

        movement *= actualSpeed * Time.fixedDeltaTime;




        playerRb.velocity = movement;
    }

    public void ResetToSpawn()
    {
        playerRb.velocity = Vector3.zero;
        transform.position = spawnPosition;
    }

//    private void OnCollisionEnter2D(Collision2D other)
//    {
//        var tag = other.gameObject.tag;
//
//        switch (tag)
//        {
//            case "Enemy" :
//                
//                
//                
//                break;
//            
//        }
//    }
//
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        var tag = other.gameObject.tag;
//
//        switch (tag)
//        {
//            case "ShadowBullet" :
//                
//                GameManager.instance.FinishRound(false);
//                
//                break;
//            
//        }
//    }
}