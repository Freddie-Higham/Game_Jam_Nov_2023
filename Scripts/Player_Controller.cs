using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    private Rigidbody2D BODY;
    private SpriteRenderer RENDERER;
   // private Animator ANIMATION;
    private BoxCollider2D PLAYER_BOX;
    private Transform TRANSFORM;
    public SpriteRenderer SPRITERENDERER;
    public Sprite dash_sprite;
    public Sprite idle_sprite;
    public Sprite death_sprite;
    public Sprite shooting_sprite;
    [SerializeField] private LayerMask PLATFORM_LAYER_MASK;

    public float x_speed;
    public float initial_x_speed;
    public float initial_y_speed;
    public float y_speed;
    private int player_y_input;
    private int player_x_input;
    private int lock_player_x_input;

    private GameObject[] checkpoints;
    private Vector3 lowest_possible_checkpoint;
    private Vector3 checkpoint_pos;

    private GameObject arrow;

    private float dash_timer;
    public float dash_decceleration;
    public bool dashing; 
    public bool direction_facing;
    private Vector3 angle;
    //public float rebounding_timer;
    public float rebound_initial_speed;
    private float rebound_speed;
    public float rebound_decceleration;
    public float rebound_length;
    private float initial_gravity;
    private float death_duration;

    public Arrow_Controller arrow_script;

    private string current_sprite;

    // Start is called before the first frame update
    void Start()
    {
        player_x_input = 0;
        player_y_input = 0;
        dash_timer = 0;
        rebound_speed = 0;

        x_speed = initial_x_speed;
        y_speed = initial_y_speed;

        BODY = GetComponent<Rigidbody2D>();
        RENDERER = GetComponent<SpriteRenderer>();
      //  ANIMATION = GetComponent<Animator>();
        PLAYER_BOX = GetComponent<BoxCollider2D>();
        TRANSFORM = GetComponent<Transform>();
        SPRITERENDERER = GetComponent<SpriteRenderer>();

        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        arrow = GameObject.FindGameObjectWithTag("Arrow");

        initial_gravity = BODY.gravityScale;
    }

    private void KeyboardInputChecks() {

        if (Input.GetKey("a")) {
            player_x_input = -1;
            RENDERER.flipX = false;
            direction_facing = false;
        } else if (Input.GetKey("d")) {
            player_x_input = 1;
            RENDERER.flipX = true;
            direction_facing = true;
        } else {
            player_x_input = 0;
        }

        if (Input.GetKey("r") && current_sprite != "shooting_sprite") {
            Debug.Log(TRANSFORM.position.x);
            current_sprite = "shooting_sprite";
            SPRITERENDERER.sprite = shooting_sprite;
            float x = TRANSFORM.position.x;
            float y = TRANSFORM.position.y;
            float z = TRANSFORM.position.z;
            arrow_script.setPosition(new Vector3(x, y, z), player_x_input);
           // Vector3 store = new Vector3(TRANSFORM.position.x, TRANSFORM.position.y, TRANSFORM.position.z);
            //arrow.GetComponent<Transform>().position = new Vector3(TRANSFORM.position.x, TRANSFORM.position.y, TRANSFORM.position.z);
         //   TRANSFORM.position = store;
        }

        if (player_x_input != 0 && Input.GetKey(KeyCode.Space) && dash_timer < -0.6f) {
            dash_timer = 0.3f;
            x_speed = initial_x_speed * 2.8f;
            lock_player_x_input = player_x_input;
            BODY.gravityScale += 1.5f;
            //ANIMATION.SetBool("Dash", true);
            SPRITERENDERER.sprite = dash_sprite;
            current_sprite = "dash_sprite";
            if (direction_facing) {
                angle = new Vector3(0, 0, 38f);
               // transform.eulerAngles = new Vector3(0, 0, 40f);
            }
            else {
                angle = new Vector3(0, 0, -38f);
                //transform.eulerAngles = new Vector3(0, 0, -40f);
            }

            dashing = true;
        }

        if (Input.GetKey("w")) {
            player_y_input = 1;
        }else {
            player_y_input = 0;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision){

        RaycastHit2D raycastHit = Physics2D.BoxCast(PLAYER_BOX.bounds.center, PLAYER_BOX.bounds.size, 0f, Vector2.down, 0.5f, PLATFORM_LAYER_MASK);

        if (collision.gameObject.tag == "Bird" && death_duration < -1f) {

            SPRITERENDERER.sprite = death_sprite;
            current_sprite = "death_sprite";
            death_duration = 0.8f;

            lowest_possible_checkpoint = new Vector3(0, 1000, 0);
            //Find closest checkpoint.
            foreach (GameObject game_object in checkpoints) {
                Debug.Log(game_object.GetComponent<Transform>().transform.position.y);
                checkpoint_pos = game_object.GetComponent<Transform>().transform.position;
                if (TRANSFORM.position.y < checkpoint_pos.y && checkpoint_pos.y < lowest_possible_checkpoint.y) {
                    lowest_possible_checkpoint = checkpoint_pos;
                }
            } 
            //Teleport to nearest checkpoint.
           // TRANSFORM.position = lowest_possible_checkpoint;
        }

        //If the ray has collided.
        if (raycastHit.collider != null){
            //If the platform below is a "trap" platform, make it dissapear.
            if (collision.gameObject.tag == "Cloud") {
               // rebound_timer = rebound_length;
                rebound_speed = rebound_initial_speed;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        death_duration -= Time.deltaTime;
        dash_timer -= Time.deltaTime;
        //rebounding_timer -= Time.deltaTime;
        if (dash_timer >= 0) {
            if (x_speed > Time.deltaTime * dash_decceleration) {
                x_speed -= Time.deltaTime * dash_decceleration;
            }
        }
        else {
            dashing = false;
            //ANIMATION.SetBool("Dash", false);
            if (current_sprite == "dash_sprite") {
                SPRITERENDERER.sprite = idle_sprite;
                current_sprite = "idle_sprite";
            }
            x_speed = initial_x_speed;
            angle = new Vector3(0, 0, 0f);
            BODY.gravityScale = initial_gravity;
        }

        if (death_duration < 0 && current_sprite == "death_sprite") {
            Debug.Log("death");
            TRANSFORM.position = lowest_possible_checkpoint;
            current_sprite = "idle_sprite";
            SPRITERENDERER.sprite = idle_sprite;
        }

        KeyboardInputChecks();

        if (dash_timer >= 0) {
          player_x_input = lock_player_x_input;
        }

        if (rebound_speed > 0) {
          player_y_input = 1;
          rebound_speed -= Time.deltaTime * rebound_decceleration;
          y_speed = rebound_speed;
        }
        else {
            y_speed = initial_y_speed;
        }

        transform.eulerAngles = angle;

        BODY.velocity = new Vector2(x_speed * player_x_input, y_speed * player_y_input);
        
    }
}
