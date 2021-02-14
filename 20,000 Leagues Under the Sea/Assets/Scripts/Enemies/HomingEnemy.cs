using UnityEngine;
using System.Collections;

public class HomingEnemy : BaseEnemy {
    
    private GameObject playerTarget; 
    private bool stopping = false;

    private bool canLock = false;
    private bool initLock = false; 
    private bool hasLocked = false;
    private float curSpeed = 0f;

    
    public GameObject lockOn; 

    public override void Start() {
        base.Start();
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("SlowDown");
    }

    public override void FixedUpdate()
    {
        if (stopping){
            canLock = true; 
            if (Mathf.Abs(rb2.velocity.x) < 0.2 ){
                rb2.velocity = new Vector2(0, rb2.velocity.y);
            }
            if (Mathf.Abs(rb2.velocity.y) < 0.1){
                rb2.velocity = new Vector2(rb2.velocity.x, 0);
            }
            rb2.velocity *= 0.95f;
        }else{
            //Regular Motion
            float verticalVel = Mathf.Sin(Time.fixedTime) * vertical * verticalMult;
            rb2.velocity = new Vector2(-maxSpeed * speedMultiplier, verticalVel);
        }        

        if (rb2.velocity == new Vector2(0,0) && canLock &&!initLock){
            //Lock on to player and chase
            initLock = true; 
            StartCoroutine("LockOn");
            if (hasLocked){
                Debug.Log("Target Locked");
            }
            canLock = false; 
        }

        if (hasLocked){
            if (curSpeed < maxSpeed){
                curSpeed += speedMultiplier;
            }
            Vector2 flyDir = (playerTarget.transform.position - this.transform.position).normalized; 
            rb2.velocity = curSpeed * flyDir; 
        }


    }


    IEnumerator LockOn(){
        float xOffset = Random.Range(-0.2f, 0.2f);
        float yOffset = Random.Range(-0.2f, 0.2f);
        GameObject lockOnClone = Instantiate(lockOn, 
                    new Vector2(playerTarget.transform.position.x + xOffset, playerTarget.transform.position.y + yOffset ), 
                    Quaternion.identity);        

        LockOn lockon = lockOnClone.GetComponent<LockOn>();
        lockon.setOffset(xOffset,yOffset);


        yield return new WaitForSeconds(0.4f);
        hasLocked = true; 
    }

    IEnumerator SlowDown(){
        yield return new WaitForSeconds(1f);
        stopping = true; 
    }   

}