using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Cloud_Controller : MonoBehaviour
{

    private BoxCollider2D BOX;
    public LayerMask PLAYER_LAYER_MASK;
    public GameObject LIGHTNING;
    private SpriteRenderer RENDERER;
    public Player_Controller PLAYER_SCRIPT;
    private Transform TRANSFORM;
    private Color starting_color;
    public float time_to_strike;
    public float lighting_effect_distance;
    public float lighting_charge_distance;
    private float strike_duration;
    private float strike_timer;
    private bool triggered;
    private float transparency;

    public bool moving;
    private float distance_travelled;
    private float RETURN_POSITION_X;
    [SerializeField] private float MAX_DISTANCE_X;
    public float speed;

    [SerializeField] private AudioSource LIGHTNING_SOUND;
    [SerializeField] private AudioSource CLOUD_DARKEN_SOUND;
    [SerializeField] private AudioSource CLOUD_BREAK_SOUND;

    void Start()
    {
        TRANSFORM = GetComponent<Transform>();
        BOX = GetComponent<BoxCollider2D>();
        RENDERER = GetComponent<SpriteRenderer>();
        //time_to_strike = 1.2f;
        RETURN_POSITION_X = TRANSFORM.position.x;
        strike_duration = 0.3f;
        strike_timer = 0f;
        starting_color = RENDERER.color;
        triggered = false;
        transparency = 0.6f;
       // Physics2D.IgnoreCollision(LIGHTNING.GetComponent<Collider2D>(), BOX);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Arrow") {
            Destroy(gameObject, 0.3f);
            triggered = true;
            CLOUD_BREAK_SOUND.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (moving) {
            distance_travelled = RETURN_POSITION_X - TRANSFORM.position.x;
            TRANSFORM.position = new Vector3(TRANSFORM.position.x - (speed * Time.deltaTime), TRANSFORM.position.y, TRANSFORM.position.z);
            if (distance_travelled > MAX_DISTANCE_X) {
                TRANSFORM.position = new Vector3(RETURN_POSITION_X, TRANSFORM.position.y, TRANSFORM.position.z);
            }
        }

        strike_timer -= Time.deltaTime;

        RaycastHit2D raycastHit = Physics2D.BoxCast(BOX.bounds.center, BOX.bounds.size, 0f, Vector2.down, lighting_charge_distance, PLAYER_LAYER_MASK);

        RaycastHit2D raycastHit_2 = Physics2D.BoxCast(BOX.bounds.center, BOX.bounds.size / 1.65f, 0f, Vector2.down, lighting_effect_distance, PLAYER_LAYER_MASK);
        
        if (raycastHit.collider != null) {
            if (!CLOUD_DARKEN_SOUND.isPlaying) {
                CLOUD_DARKEN_SOUND.Play();
            }
            if (RENDERER.color.r > 0.2f) {
                RENDERER.color = new Color(RENDERER.color.r - Time.deltaTime, RENDERER.color.b - Time.deltaTime, RENDERER.color.g - Time.deltaTime, 1f); 
            }
            time_to_strike -= Time.deltaTime;
        }
        else if (time_to_strike > 0.6f) {
            time_to_strike = 1.2f;
            RENDERER.color = starting_color;
        }
        else {
            time_to_strike -= Time.deltaTime;
        }

        if (time_to_strike < 0) {
            if (raycastHit_2.collider != null) {
                PLAYER_SCRIPT.die();
            }
            LIGHTNING.GetComponent<Transform>().position = BOX.bounds.center - new Vector3(0, 1.3f, 0);
            LIGHTNING_SOUND.Play();
            time_to_strike = 2f;
            strike_timer = strike_duration;
        }

        if (strike_timer < 0) {
            LIGHTNING.GetComponent<Transform>().position = new Vector3(100, 100, 0);
        }

        if (triggered) {
            transparency -= 0.003f;
            RENDERER.color = new Color(RENDERER.color.r, RENDERER.color.b, RENDERER.color.g, transparency); 
        }
    }
}