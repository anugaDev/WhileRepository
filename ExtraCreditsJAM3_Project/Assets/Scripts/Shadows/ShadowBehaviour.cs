﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform bulletPrefab;
    [SerializeField] private float 
        shadowSpeed,
        shadowRotSpeed,
        inPositionRadius,
        startMovingTime,
        startingWaitTime,
        loopPauseTime,
        bulletSpeed,
        timeBetweenChangingPositions
        ;

    private float timeForPosChange = 0;
    

    [HideInInspector] public RoundRecordContainer shadowActionsRecord;

    private MovementRecord actualPositionTarget;
    private ShootingRecord actualInQeueAction;

    private Rigidbody2D shadowRb;
    private bool isMoving = false, allActionsMade, allPositionsMade;
    
    // Start is called before the first frame update
    void Start()
    {
        shadowRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            UpdateMovement();
            UpdateRotation();
            CheckForPosition();
            CheckForShooting();

            if (allActionsMade && allPositionsMade)
            {
                ResetShadow();
            }
        }
        print(timeBetweenChangingPositions);
    }
    public void SetBehaviour(RoundRecordContainer _shadowActionsRecord)
    {
       
        
        this.shadowActionsRecord = _shadowActionsRecord;
        
        actualPositionTarget = shadowActionsRecord.roundMovementRecords[0];
        if (_shadowActionsRecord.roundShootingRecords.Count > 0)
            actualInQeueAction = shadowActionsRecord.roundShootingRecords[0];
        else allActionsMade = true;
        
        startMovingTime = Time.time;

        transform.position = actualPositionTarget.position;

        timeBetweenChangingPositions = GameManager.instance.playerRec.recordTime;

        StartCoroutine(WaitTime(startingWaitTime));

        allPositionsMade = false;


    }

    private void UpdateMovement()
    {
        var direction = actualPositionTarget.position - transform.position;
        direction.Normalize();
        
        
        
        if ((actualPositionTarget.position - transform.position).magnitude > inPositionRadius)
        {
            shadowRb.velocity = direction * shadowSpeed * Time.fixedDeltaTime;
        }
        else
        {
            shadowRb.velocity = Vector3.zero;
        }
       
        
    }

    private void UpdateRotation()
    {
      
        var rotationZ = Mathf.Atan2(actualPositionTarget.direction.y, actualPositionTarget.direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        
       

    }

    private void CheckForPosition()
    {
        if (!allPositionsMade)
        {
            
            timeForPosChange += Time.deltaTime;
            if (!(timeForPosChange >= timeBetweenChangingPositions)) return;
            if (actualPositionTarget ==
                shadowActionsRecord.roundMovementRecords[shadowActionsRecord.roundMovementRecords.Count - 1])
            {
                allPositionsMade = true;
            }
            else
            {
                SetNextPosition();
            }

            timeForPosChange = 0;
        }

    }
    private void CheckForShooting()
    {
        var actualTime = Time.time;

       
        if (!allActionsMade)
        {
            if ((actualTime - startMovingTime) >= actualInQeueAction.instant)
            {
                ShadowShoot();

            }
        }
       


    }

    private void SetNextPosition()
    {
        actualPositionTarget =
            shadowActionsRecord.roundMovementRecords
            [shadowActionsRecord.roundMovementRecords.IndexOf(actualPositionTarget) + 1];
    }
    private void SetNextAction()
    {
        actualInQeueAction =
            shadowActionsRecord.roundShootingRecords
                [shadowActionsRecord.roundShootingRecords.IndexOf(actualInQeueAction) + 1];
    }

    

    private void ShadowShoot()
    {
        var rotationZ = Mathf.Atan2(actualInQeueAction.shootingDir.y, actualInQeueAction.shootingDir.x) * Mathf.Rad2Deg;
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0.0f, 0.0f, rotationZ)).GetComponent<BulletBehaviour>();
        bullet.SetBullet(bulletSpeed,actualInQeueAction.shootingDir,bullet.GetComponent<Rigidbody2D>());
        
        if (actualInQeueAction ==
            shadowActionsRecord.roundShootingRecords[shadowActionsRecord.roundShootingRecords.Count - 1])
        {
            
            allActionsMade = true;
        }
        else
        {
           
            SetNextAction();
        }
    }

   

    



    private void ResetShadow()
    {
        
        actualPositionTarget = shadowActionsRecord.roundMovementRecords[0];
        if (shadowActionsRecord.roundShootingRecords.Count > 0)
        {
            actualInQeueAction = shadowActionsRecord.roundShootingRecords[0];
            allActionsMade = false;
        }
           
        else allActionsMade = true;

        shadowRb.velocity = Vector3.zero;
        
        StartCoroutine (WaitTime(loopPauseTime));

        allPositionsMade = false;

    }

    IEnumerator WaitTime(float _waitTime)
    {
       
        isMoving = false;
        yield return new WaitForSeconds(_waitTime);
        isMoving = true;
        startMovingTime = Time.time;
        
        transform.position = actualPositionTarget.position;
       
        timeForPosChange = 0;
    }
}