using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D BODY;
    private SpriteRenderer RENDERER;
   // private Animator ANIMATION;
    private BoxCollider2D PLAYER_BOX;
    private Transform TRANSFORM;
    public SpriteRenderer SPRITERENDERER;
    public Sprite dash_sprite;
    public Sprite dash_sword_sprite;
    public Sprite idle_sprite;
    public Sprite death_sprite;
    public Sprite shooting_sprite;
    public Sprite shot_sprite;
    [SerializeField] private LayerMask PLATFORM_LAYER_MASK;

    public float x_speed;
    public float initial_x_speed;
    public float initial_y_speed;
    public float y_speed;
    private float player_y_input;
    private float player_x_input;

    private GameObject[] checkpoints;
    private Vector3 lowest_possible_checkpoint;
    private Vector3 checkpoint_pos;

    private GameObject arrow;

    private float dash_timer;
    public float dash_decceleration;
    public bool dashing_x; 
    public bool dashing_y; 
    public bool direction_facing_x;
    public bool direction_facing_y;
    private Vector3 angle;
    //public float rebounding_timer;
    public float rebound_initial_speed;
    private float rebound_speed;
    public float rebound_decceleration;
    public float rebound_length;
    private float initial_gravity;
    private float death_duration;
    private float game_timer;
    private float timer_time;

    public int level_number;

    private float shooting_animation_timer;

    public Arrow_Controller arrow_script;
    public Level_Complete_Controller LEVEL_COMPLETE_SCRIPT;
    public Camera_Controller CAMERA_SCRIPT;

    private string current_sprite;

    public int arrow_charges;
    public int dash_charges;

    public bool space_up;

    public bool wet;
    private bool complete;

    private float dash_charges_timer;
    private float arrow_charges_timer;
    public float max_dash_charge;
    private float max_arrow_charge; 

    private float countdown_timer;
    private int countdown_value;
    private bool counting_down;

    [SerializeField] private AudioSource DASH_SOUND;
    [SerializeField] private AudioSource ARROW_FIRE_SOUND;
    [SerializeField] private AudioSource CLOUD_BOUNCE_SOUND;
    [SerializeField] private AudioSource DEATH_SOUND;
    [SerializeField] private AudioSource CHARGE_GAINED_1_SOUND;
    [SerializeField] private AudioSource CHARGE_GAINED_2_SOUND;
    [SerializeField] private AudioSource START_SOUND_1;
    [SerializeField] private AudioSource START_SOUND_2;
    [SerializeField] private AudioSource FINISH_SOUND;
    [SerializeField] private AudioSource FAIL_SOUND;
    [SerializeField] private AudioSource BACKGROUND_MUSIC;

    public GameObject level_complete_controller;
    public GameObject level_failed;

    public TextMeshProUGUI timer_text;
    public TextMeshProUGUI countdown_text;

    // Start is called before the first frame update
    void Start()
    {
        player_x_input = 0;
        player_y_input = 0;
        dash_timer = 0;
        rebound_speed = 0;

        dash_charges_timer = 0;
        arrow_charges_timer = 0;
        countdown_timer = -0.5f;
        countdown_value = 0;

        shooting_animation_timer = 0;

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

        Cursor.visible = false;

        initial_gravity = BODY.gravityScale;

        space_up = true;
        complete = false;

        counting_down = true;

        arrow_charges = 0;
        dash_charges = 0;
        if (level_number == 1) {
            game_timer = Level_1.time_limit;
        }
        else if (level_number == 2) {
            game_timer = Level_2.time_limit;
        }
        else if (level_number == 3) {
            game_timer = Level_3.time_limit;
        }

    }

    private void KeyboardInputChecks() {

        if (dash_timer >= 0) {
            //locking the x_input while dashing.
        }
        //Left-Right movement controls
        else{
            if (Input.GetKey("a")) {
                player_x_input = -1;
                RENDERER.flipX = false;
                direction_facing_x = false;
            } else if (Input.GetKey("d")) {
                player_x_input = 1;
                RENDERER.flipX = true;
                direction_facing_x = true;
            } else {
                player_x_input = 0;
            }
        }

        if (dash_timer >= 0) {}
        else{
            //Up_down movement controls
            if (Input.GetKey("w")) {
                //player_y_input = 0.01f;
              //  BODY.gravityScale = initial_gravity / 1.5f;
                direction_facing_y = true;
            }else {
                direction_facing_y = false;
            }
            player_y_input = -0.2f;
            BODY.gravityScale = initial_gravity;
        }

        if (Input.GetKey("r") && shooting_animation_timer <= 0.05f && arrow_charges >= 1 && current_sprite != "death_sprite") {
            current_sprite = "shooting_sprite";
            SPRITERENDERER.sprite = shooting_sprite;
            TRANSFORM.position = new Vector3(TRANSFORM.position.x + (0.15f * (direction_facing_x ? -1 : 1)), TRANSFORM.position.y + 0.15f, TRANSFORM.position.z);
            float x = TRANSFORM.position.x;
            float y = TRANSFORM.position.y - 0.1f;
            float z = TRANSFORM.position.z;

            BODY.gravityScale = 0;

            ARROW_FIRE_SOUND.Play();

            arrow_script.setPosition(new Vector3(x, y, z), direction_facing_x);

            shooting_animation_timer = 1f;

            arrow_charges -= 1;

            arrow_charges_timer = 0;

           // Vector3 store = new Vector3(TRANSFORM.position.x, TRANSFORM.position.y, TRANSFORM.position.z);
            //arrow.GetComponent<Transform>().position = new Vector3(TRANSFORM.position.x, TRANSFORM.position.y, TRANSFORM.position.z);
         //   TRANSFORM.position = store;
        }

        if (shooting_animation_timer < 0 && Input.GetKey(KeyCode.Space) && dash_timer < 0.1f && dash_charges >= 1 && space_up && current_sprite != "death_sprite" && !wet) {

            DASH_SOUND.Play();

            dash_charges -= 1;

            dash_charges_timer = 0;

            if (player_x_input != 0) {
                dash_timer = 0.3f;
                x_speed = initial_x_speed * (Upgrades.upgrades_status[0,0] ? 3f : 2.7f);
                //ANIMATION.SetBool("Dash", true);
                if (Upgrades.upgrades_status[0, 2] == true) {
                    PLAYER_BOX.size *= 1.5f;
                    SPRITERENDERER.sprite = dash_sword_sprite;
                }
                else {
                    SPRITERENDERER.sprite = dash_sprite;
                }
                current_sprite = "dash_sprite";
                if (direction_facing_x) {
                    if (Input.GetKey("w")) {
                        angle = new Vector3(0, 0, 67f);
                        player_y_input = 1f;
                    }
                    else if (Input.GetKey("s")) {
                        angle = new Vector3(0, 0, 23f);
                        BODY.gravityScale *= 2;
                    }
                    else {
                        BODY.gravityScale /= 3;
                        angle = new Vector3(0, 0, 40f);
                    }
                // transform.eulerAngles = new Vector3(0, 0, 40f);
                }
                else {
                    if (Input.GetKey("w")) {
                        angle = new Vector3(0, 0, -67f);
                        player_y_input = 1f;
                    }
                    else if (Input.GetKey("s")) {
                        angle = new Vector3(0, 0, -23f);
                        BODY.gravityScale *= 2f;
                    }
                    else {
                        BODY.gravityScale /= 3;
                        angle = new Vector3(0, 0, -40f);
                    }
                    //transform.eulerAngles = new Vector3(0, 0, -40f);
                }

                dashing_x = true;
            }

            else if (player_y_input != 0f && rebound_speed <= 0) {
                RENDERER.flipX = false;
                dash_timer = 0.3f;
                y_speed += (initial_x_speed * (Upgrades.upgrades_status[0,0] ? 1.4f : 0.9f));
                //ANIMATION.SetBool("Dash", true);
                if (Upgrades.upgrades_status[0, 2] == true) {
                    SPRITERENDERER.sprite = dash_sword_sprite;
                    PLAYER_BOX.size *= 1.5f;
                }
                else {
                    SPRITERENDERER.sprite = dash_sprite;
                }
                current_sprite = "dash_sprite";
                if (direction_facing_y) {
                    angle = new Vector3(0, 0, -137f); 
                    player_y_input = 1f;
                // transform.eulerAngles = new Vector3(0, 0, 40f);
                }
                else {
                    angle = new Vector3(0, 0, 43f);
                    player_y_input = -1f;
                    //transform.eulerAngles = new Vector3(0, 0, -40f);
                }

                dashing_y = true;
            }
        }

        if (Input.GetKeyDown("space")) {
            space_up = false;
        }

        if (Input.GetKeyDown("t")) {
            game_timer -= 5;
        }

        if (Input.GetKeyDown("p")) {
            LEVEL_COMPLETE_SCRIPT.LevelCompleteInfo(game_timer, level_number);
            level_complete_controller.SetActive(true);
            complete = true;
            BODY.gravityScale = 0.005f;
        }

        if (Input.GetKeyDown("q")) {
            SceneManager.LoadScene("Level_Select_Menu");
        }

        if (!space_up && Input.GetKeyUp("space")) {
            space_up = true;
        }


    }

    public void die() {

        //Debug.Log($"Death check: {current_sprite} != 'dash_sprite' && {Upgrades.upgrades_status[0, 2]} != true");

        if (!(current_sprite == "dash_sprite" && Upgrades.upgrades_status[0, 2] == true)) {

            DEATH_SOUND.Play();

            SPRITERENDERER.sprite = death_sprite;
            current_sprite = "death_sprite";
            death_duration = 0.8f;

            lowest_possible_checkpoint = new Vector3(0, 1000, 0);
            //Find closest checkpoint.
            foreach (GameObject game_object in checkpoints) {
                checkpoint_pos = game_object.GetComponent<Transform>().transform.position;
                if (TRANSFORM.position.y < checkpoint_pos.y && checkpoint_pos.y < lowest_possible_checkpoint.y) {
                    lowest_possible_checkpoint = checkpoint_pos;
                }
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision){

        RaycastHit2D raycastHit = Physics2D.BoxCast(PLAYER_BOX.bounds.center - new Vector3(0f, 0.8f, 0f), PLAYER_BOX.bounds.size, 0f, Vector2.down, 0.4f, PLATFORM_LAYER_MASK);

        rebound_speed = 0;

        //Collision with a bird will cause death.
        if (collision.gameObject.tag == "Bird" && death_duration < -1f) {
            //Debug.Log("Collision with bird.");
            die();
        }

        if (collision.gameObject.tag == "Lightning_Cloud" && death_duration < -1f) {
            die();
        }

        if (collision.gameObject.tag == "Finish_Line" && !complete) {
            FINISH_SOUND.Play();
            BACKGROUND_MUSIC.Stop();
            LEVEL_COMPLETE_SCRIPT.LevelCompleteInfo(game_timer, level_number);
            level_complete_controller.SetActive(true);
            complete = true;
            BODY.gravityScale = 0f;
            BODY.velocity = new Vector2(0,0);
        }

        //Bouncing off a cloud.
        if (raycastHit.collider != null) {
            if (collision.gameObject.tag == "Cloud" && !(current_sprite == "dash_sprite" && Upgrades.upgrades_status[0,2] == true)) {
                // rebound_timer = rebound_length;
                CLOUD_BOUNCE_SOUND.Play();
                rebound_speed = rebound_initial_speed;
            }
            if (collision.gameObject.tag == "Balloon") {
                // rebound_timer = rebound_length;
                CLOUD_BOUNCE_SOUND.Play();
                rebound_speed = rebound_initial_speed;
            }
        }

    }

    private void Countdown() {

        if (countdown_timer >= 0) {
            if (!START_SOUND_2.isPlaying) {
                START_SOUND_2.Play();
            }
        }

        //START_SOUND_1.Play();

        countdown_timer += Time.deltaTime;

        if (countdown_timer <= 3) {
            if (countdown_timer >= countdown_value + 1) {
                if (countdown_timer <= 2) {
                    //START_SOUND_1.Play();
                }
                else {
                    //START_SOUND_2.Play();
                }
                countdown_value += 1;
                countdown_text.text = $"{3 - countdown_value}";
            }
        }
        else {
            //START_SOUND_2.Play();
            countdown_text.text = "";
            counting_down = false;
            BODY.gravityScale *= 2;
            BACKGROUND_MUSIC.Play();
            CAMERA_SCRIPT.levelBegun();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Upgrades.upgrades_status[0, 1] == true) {
            max_dash_charge = 3;
        }
        else {
            max_dash_charge = 2;
        }

        if (Upgrades.upgrades_status[1, 1] == true) {
            max_arrow_charge = 2;
        }
        else {
            max_arrow_charge = 1;
        }

        if (counting_down) {
            BODY.velocity = new Vector2(0, -0.02f);
            //BODY.gravityScale /= 2;
            Countdown();
        }
        if (!complete && !counting_down) {

        death_duration -= Time.deltaTime;
        dash_timer -= Time.deltaTime;
        shooting_animation_timer -= Time.deltaTime;
        game_timer -= Time.deltaTime;
        dash_charges_timer += Time.deltaTime;
        arrow_charges_timer += Time.deltaTime;

        string minute = Math.Floor(game_timer/60).ToString("00");
        string seconds = (game_timer % 60).ToString("00");
        timer_text.text = $"{minute}:{seconds}";

        if (game_timer <= 40 && game_timer > 20) { 
            timer_text.color = new Color(210f/255f, 132f/255f, 58f/255f, 255f);
        }       
        else if (game_timer <= 20) { 
            timer_text.text = $"<b>{timer_text.text}</b>";
            timer_text.color = new Color(255f/255f, 0f, 0f, 255f);
        }       

        if ((dash_charges_timer > (Upgrades.upgrades_status[0,1] ? 2.1f : 2.4f)) && dash_charges < max_dash_charge) {
            CHARGE_GAINED_2_SOUND.Play();
            dash_charges += 1;
            dash_charges_timer = 0;
        }                       

        if ((arrow_charges_timer > (Upgrades.upgrades_status[1,1] ? 2.3f : 2.6f)) && arrow_charges < max_arrow_charge) {
            CHARGE_GAINED_1_SOUND.Play();
            arrow_charges += 1;
            arrow_charges_timer = 0;
        }   

        if (shooting_animation_timer < 0.5f && shooting_animation_timer > 0f) {
            SPRITERENDERER.sprite = idle_sprite;
            current_sprite = "idle_sprite";
            BODY.gravityScale = initial_gravity;
        }
        else if (shooting_animation_timer < 0.8f && shooting_animation_timer > 0.2f) {
            SPRITERENDERER.sprite = shot_sprite;
            current_sprite = "shooting_sprite";
        }


        //rebounding_timer -= Time.deltaTime;
        if (dash_timer >= 0 && dashing_x) {
            if (x_speed > Time.deltaTime * dash_decceleration) {
                x_speed -= Time.deltaTime * dash_decceleration;
            }
        }
        else if (dash_timer >= 0 && dashing_y) {
            if (y_speed > Time.deltaTime * dash_decceleration) {
                y_speed -= Time.deltaTime * dash_decceleration;
            }
        }
        else {
            dashing_x = false;
            dashing_y = false;
            //ANIMATION.SetBool("Dash", false);
            if (current_sprite == "dash_sprite") {
                SPRITERENDERER.sprite = idle_sprite;
                current_sprite = "idle_sprite";
                PLAYER_BOX.size /= 1.5f;
            }
            x_speed = initial_x_speed;
            y_speed = initial_x_speed;
            angle = new Vector3(0, 0, 0f);
            BODY.gravityScale = initial_gravity;
        }

        if (death_duration < 0 && current_sprite == "death_sprite") {
            TRANSFORM.position = lowest_possible_checkpoint;
            current_sprite = "idle_sprite";
            SPRITERENDERER.sprite = idle_sprite;
        }

        KeyboardInputChecks();

        if (rebound_speed > 0) {
          player_y_input = 1;
          rebound_speed -= Time.deltaTime * rebound_decceleration;
          y_speed = rebound_speed;
        }
        else if (!dashing_y) {
            y_speed = initial_y_speed;
        }

        transform.eulerAngles = angle;

        if (wet){
            BODY.velocity = new Vector2((x_speed * player_x_input / 2), y_speed * player_y_input);
        }
        else{
            BODY.velocity = new Vector2(x_speed * player_x_input, y_speed * player_y_input);
        }

        if (game_timer <= 0.1f) {
            complete = true;
            BODY.gravityScale = 0.01f;
            BODY.velocity = new Vector2(0,0);
            level_failed.SetActive(true);
            BACKGROUND_MUSIC.Stop();
            FAIL_SOUND.Play();
            Cursor.visible = true;
        }

        }
        
    }
}