using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public MapLimits limits;
    public float speed;
    public float changeTimer;
    public float shootPower;
    public int scoreReward;
    float maxTimer;
    public int enemyHp;
    public bool directionSwitch;
    public bool canShoot;
    public Transform enenmyShootingPosition;
    public GameObject enemyBullet;
    public GameObject loot;
    float shootTimer;
    float maxShootTimer;
    public GameObject particleEffect;
    public GameObject enemyDeath;
    Rigidbody rig;
    void Start()
    {
        shootTimer = Random.Range(0, 4);
        maxShootTimer = shootTimer;
        // assign the max timer to the current change timer set for the game
        maxTimer = changeTimer;
        // references the rigidbody component attached to the object
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // when we hit the left boundary we activate the switch direction method
        if (transform.position.x == (limits.minX)) SwitchDirection(directionSwitch);
        if (transform.position.x == (limits.maxX)) SwitchDirection(directionSwitch);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, limits.minX, limits.maxX),
        Mathf.Clamp(transform.position.y, limits.minY, limits.maxY),
        0);
        Movement();
        switchTimer();

        shootTimer -= Time.deltaTime;
        if(canShoot)
            if(shootTimer <= 0)
            {
                GameObject newBullet = Instantiate(enemyBullet, transform.position, transform.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = Vector3.up*2 * -shootPower;
                shootTimer = maxShootTimer;
            }
    }

    void Movement()
    {
        // when direction switch is true we move in right direction on x axis, if not we move left
        if(directionSwitch)
        rig.velocity = new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0);
        else
            rig.velocity = new Vector3(-speed * Time.deltaTime, -speed * Time.deltaTime, 0);

    }

    // method to invert direction
    void switchTimer()
    {
        changeTimer -= Time.deltaTime;
        if(changeTimer < 0)
        {
            // when the change timer is less than 0 we switch direction
            SwitchDirection(directionSwitch);
            // after changing direction, we reset changeTimer to the original value 
            changeTimer = maxTimer;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        // if the enemy bullet hits an enemy nothing happens
        if (col.gameObject.tag == "enemyBullet")
            return;
        if (col.gameObject.tag == "friendlyBullet")
        {
            Destroy(col.gameObject); // destroy collided game object bullet
            enemyHp--; // loose 1 hp
            Instantiate(particleEffect, transform.position, transform.rotation); // when enemy is hit, a particle effect is formed here

            if (enemyHp <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>().score += scoreReward; // get player obj because it is not the same tag in if state
                Destroy(gameObject); // destroy enemy object
                Instantiate(enemyDeath, transform.position, transform.rotation); // when enemy is dead, a particle effect is formed here(add death timer to the effect)
                if (canShoot)
                {
                    GameObject newLoot = Instantiate(loot, enenmyShootingPosition.transform.position, enenmyShootingPosition.transform.rotation);
                }
                else return;
            }
        }

        // if the object this script is attached to collides with player
        if (col.gameObject.tag == "Player")
        {
            Instantiate(particleEffect, transform.position, transform.rotation); // when collide with player show dmg
            col.gameObject.GetComponent<PlayerCharacter>().hp--; // player loose 1 hp
            //col.gameObject.GetComponent<PlayerCharacter>().he--; // player loose 1 hp
            enemyHp--;
            if (enemyHp <= 0)
            {
                col.gameObject.GetComponent<PlayerCharacter>().score += scoreReward; // adds to the score of player
                Destroy(gameObject);
            }
        }
    }

    // change current direction when this method is called
    bool SwitchDirection(bool direction)
    {
        if (direction) directionSwitch = false;
        else directionSwitch = true;

        return directionSwitch;
    }
}
