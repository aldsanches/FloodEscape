using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    //float
    public float maxSpeed = 3;
    public float speed = 50f;
    public float jumpPower = 200f;

    //booleans
    public bool grounded;
    public bool canDoubleJump;

    //stats
    public int curHealth;
    public int maxHealth = 5;

    //references
    private Rigidbody2D rb2d;
    private Animator anim;

    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        curHealth = maxHealth;
	}
	
	void Update () {
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x)); //Mathf.Abs = pega o valor absoluto, modulo.

        if(Input.GetAxis("Horizontal") < -1.0f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetAxis("Horizontal") > 1.0f) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        //Pulo e pulo duplo
        if (Input.GetButtonDown("Jump")) {
            if (grounded) {
                rb2d.AddForce(Vector2.up * jumpPower);
                canDoubleJump = true;
            } else {
                if (canDoubleJump) {
                    canDoubleJump = false;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    rb2d.AddForce(Vector2.up * jumpPower / 1.75f);
                }
            }
        }
		if (curHealth > maxHealth) {
			curHealth = maxHealth;
		}

		if (curHealth <= 0) {
			Die();
		}
    }

    void FixedUpdate() {
        Vector3 easeVelocity = rb2d.velocity;
        easeVelocity.y = rb2d.velocity.y;
        easeVelocity.z = 0.0f;
        easeVelocity.x *= 0.75f;

        //Simulando a fricção do player
        if (grounded) {
            rb2d.velocity = easeVelocity; 
        }

        //Movendo o player
        float h = Input.GetAxis("Horizontal");
        rb2d.AddForce((Vector2.right * speed) * h); 
        
        //Limitando a velocidade do player
        if (rb2d.velocity.x > maxSpeed) {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }

        if (rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }
    }
	//Restart
    void Die() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void Damage(int dmg) {
        curHealth -= dmg;
        gameObject.GetComponent<Animation>().Play("Player_RedFlash");

    }

    public IEnumerator Knockback(float knockDur, float knockbackPwr, Vector3 knockbackDir) {
        float timer = 0;

        while (knockDur > timer) {
            timer += Time.deltaTime;
            rb2d.velocity = new Vector2(0, 0);
            rb2d.AddForce(new Vector3(knockbackDir.x * -100, -knockbackDir.y * knockbackPwr, transform.position.z));
        }
        yield return 0; //quando o while acaba, o IENumerator precisa de um retorno.
    }
}

