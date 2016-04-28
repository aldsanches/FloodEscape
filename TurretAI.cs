using UnityEngine;
using System.Collections;

public class TurretAI : MonoBehaviour {

    //Integers
    public int curHealth;
    public int maxHealth;

    //Floats
    public float distance;
    public float wakeRange;
    public float shootInterval;
    public float bulletSpeed = 100;
    public float bulletTimer;

    //Booleans
    public bool awake = false;
    public bool lookingRight = true;

    //References
    public GameObject bullet;
    public Transform target;
    public Animator anim;
    public Transform shootPointLeft, shootPointRight;

    void Awake() {
        anim = gameObject.GetComponent<Animator>();
    }

    void Start() {
        curHealth = maxHealth;
    }

    void Update() {
        anim.SetBool("Awake", awake);
        anim.SetBool("LookingRight", lookingRight);

        RangeCheck();

        if(target.transform.position.x < transform.position.x) {
            lookingRight = true;
        }
        if (target.transform.position.x > transform.position.x) {
            lookingRight = false;
        }
        if (curHealth <= 0) {
            Destroy(gameObject);
        }
    }

    void RangeCheck() {
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < wakeRange) {
            awake = true;
        }
        if (distance > wakeRange) {
            awake = false;
        }
    }

    public void Attack(bool attackingRight) {
        bulletTimer += Time.deltaTime; //incrementa 1 por segundo

        if (bulletTimer >= shootInterval) {
            Vector2 direction = target.transform.position - transform.position;
            direction.Normalize();

            if (!attackingRight) {
                GameObject bulletClone;
                bulletClone = Instantiate(bullet, shootPointLeft.position, shootPointLeft.rotation) as GameObject; //"as GameObject" é o cast
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;
            }

            if (attackingRight) {
                GameObject bulletClone;
                bulletClone = Instantiate(bullet, shootPointRight.position, shootPointRight.rotation) as GameObject; //"as GameObject" é o cast
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;
            }
        }
    }

    public void Damage(int damage) {
        curHealth -= damage;
        gameObject.GetComponent<Animator>().Play("Player_RedFlash");
    }
}

