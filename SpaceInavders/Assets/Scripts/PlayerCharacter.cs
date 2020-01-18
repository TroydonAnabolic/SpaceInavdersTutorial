using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public float movementSpeed;
    public MapLimits limits;
    public GameObject bullet;
    public GameObject particleEffect;
    public Transform pos1;
    public Transform posL;
    public Transform posR;
    public AudioSource audioS;
    public AudioClip shotSound;
    public float shotPower;
    public int hp;
    int power;
    public Text scoreText;
    public Text healthText;
    public int score;
    int highScore;
    // Start is called before the first frame update
    void Start()
    {
        power = 1;
        if (!PlayerPrefs.HasKey("highscore"))
        {
            // when the game is started the high score is set to 0 if the player has none, then creates it
            highScore = 0;
            PlayerPrefs.SetInt("highscore", highScore);
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = hp.ToString();
        scoreText.text = score.ToString();

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highscore", highScore);
        }

        // destroy player when hp less than 0
        if (hp <= 0)
        {
            Destroy(gameObject);
            Application.Quit();
        }
        Movement();
        // limits the player to move within view limits
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, limits.minX, limits.maxX),
            Mathf.Clamp(transform.position.y, limits.minY, limits.maxY),
            0);
        Shooting();
        audioS = GetComponent<AudioSource>();
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * -movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.up * -movementSpeed * Time.deltaTime);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioS.PlayOneShot(shotSound);
            switch (power)
            {
                case 1:
                    {
                        GameObject newBullet = Instantiate(bullet, pos1.position, transform.rotation); // create new bullet at position one's position
                        newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower; // sets velocity using vector3 and shot power
                    }
                    break;
                case 2:
                    {
                        GameObject bullet1 = Instantiate(bullet, posL.position, transform.rotation);
                        bullet1.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower; // sets velocity using vector3 and shot power
                        GameObject bullet2 = Instantiate(bullet, posR.position, transform.rotation);
                        bullet2.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower; // sets velocity using vector3 and shot power
                    }
                    break;
                case 3:
                    {
                        GameObject bullet1 = Instantiate(bullet, posL.position, transform.rotation);
                        bullet1.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower; // sets velocity using vector3 and shot power
                        GameObject bullet2 = Instantiate(bullet, posR.position, transform.rotation);
                        bullet2.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower; // sets velocity using vector3 and shot power
                        GameObject bullet3 = Instantiate(bullet, pos1.position, transform.rotation);
                        bullet3.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower; // sets velocity using vector3 and shot power
                    }
                    break;
                default:
                    {

                        GameObject newBullet = Instantiate(bullet, pos1.position, transform.rotation); // create new bullet at position one's position
                        newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower; // sets velocity using vector3 and shot power
                    }
                    break;
            }
        }
    }

    // colliding with a power up method
    void OnTriggerEnter(Collider col)
    {
        // if an object collides with a power up
        if (col.gameObject.tag == "powerUp")
        {
            if (power < 3)
            {
                power++;
                Destroy(col.gameObject);
            }
        }
        // if an object collides with a power down
        if (col.gameObject.tag == "powerDown")
        {
            if (power > 1)
            {
                power--;
                Destroy(col.gameObject);
            }
        }

        if (col.gameObject.tag == "enemyBullet")
        {
            Destroy(col.gameObject); // destroy collided game object bullet
            hp--; // loose 1 hp
            Instantiate(particleEffect, transform.position, transform.rotation);

            //if (hp <= 0)
            //{
            //    Destroy(gameObject); // destroy player object
            //}
        }
    }
}
