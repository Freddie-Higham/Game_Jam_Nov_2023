using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_Cloud_Controller : MonoBehaviour
{
    private Transform TRANSFORM;
    private SpriteRenderer RENDERER;
    private BoxCollider2D BOX;
    public LayerMask PLAYER_LAYER_MASK;
    public Player_Controller PLAYER_SCRIPT;
    private float distance_travelled;
    private float RETURN_POSITION_X;
    [SerializeField] private float MAX_DISTANCE_X;
    public float speed;
    private bool triggered;
    private float transparency;
    public bool moving;

    private bool wet_from_this_cloud;

    [SerializeField] private AudioSource CLOUD_BREAK_SOUND;

    void Start() {
        TRANSFORM = GetComponent<Transform>();
        BOX = GetComponent<BoxCollider2D>();
        RETURN_POSITION_X = TRANSFORM.position.x;
        triggered = false;
        transparency = 0.6f;
        RENDERER = GetComponent<SpriteRenderer>();
        wet_from_this_cloud = false;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Arrow") {
            CLOUD_BREAK_SOUND.Play();
            Destroy(gameObject, 0.3f);
            triggered = true;
        }
    }

    void Update() {

        if (moving) {
            distance_travelled = RETURN_POSITION_X - TRANSFORM.position.x;
            TRANSFORM.position = new Vector3(TRANSFORM.position.x - (speed * Time.deltaTime), TRANSFORM.position.y, TRANSFORM.position.z);
            if (distance_travelled > MAX_DISTANCE_X) {
                TRANSFORM.position = new Vector3(RETURN_POSITION_X, TRANSFORM.position.y, TRANSFORM.position.z);
            }
        }

        RaycastHit2D raycastHit = Physics2D.BoxCast(BOX.bounds.center, BOX.bounds.size, 0f, Vector2.down, 3f, PLAYER_LAYER_MASK);

        if (raycastHit.collider != null) {
            PLAYER_SCRIPT.wet = true;
            wet_from_this_cloud = true;
        }
        else if (wet_from_this_cloud){
            PLAYER_SCRIPT.wet = false;
            wet_from_this_cloud = false;
        }

        if (triggered) {
            transparency -= 0.003f;
            RENDERER.color = new Color(RENDERER.color.r, RENDERER.color.b, RENDERER.color.g, transparency); 
        }
    }
}
