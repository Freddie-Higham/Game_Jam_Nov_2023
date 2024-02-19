using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    private Transform TRANSFORM;
    private SpriteRenderer RENDERER;
    public GameObject PLAYER;
    public Player_Controller PLAYER_SCRIPT;
    [SerializeField] private AudioSource CHARGE_GAIN;
    public float direction_facing;
    private float initial_x_speed;
    private float initial_y_speed;
    public float x_speed;
    public float y_speed;

    public void setPosition(Vector3 pos, bool d) {
        TRANSFORM.position = pos;
        if (d) {
            direction_facing = 1;
        }
        else {
            direction_facing = -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag != null) {
            TRANSFORM.position = new Vector3(100,100,100);
            if (collision.gameObject.tag == "Bird" || collision.gameObject.tag == "Cloud" || collision.gameObject.tag == "Lightning_Cloud" || 
            collision.gameObject.tag == "Dark_Cloud" || collision.gameObject.tag == "Balloon") {
                if (Upgrades.upgrades_status[1,2] == true) {
                    if (PLAYER_SCRIPT.dash_charges < PLAYER_SCRIPT.max_dash_charge && Random.Range(1,2) == 1){
                        PLAYER_SCRIPT.dash_charges += 1;
                        CHARGE_GAIN.Play();
                        Debug.Log("Gained");
                    }
                    else {
                        Debug.Log("Failed");
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RENDERER = GetComponent<SpriteRenderer>();
        TRANSFORM = GetComponent<Transform>();

        Physics2D.IgnoreCollision(PLAYER.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
        initial_x_speed = x_speed;
        initial_y_speed = y_speed;
    }

    // Update is called once per frame
    void Update()
    {

        if (direction_facing == 1) {
            RENDERER.flipX = true;
        }
        else {
            RENDERER.flipX = false;
        }

        float speed_modifier = Upgrades.upgrades_status[1,0] ? 0.9f : 0f;

        TRANSFORM.position = new Vector3(TRANSFORM.position.x + ((x_speed + speed_modifier) * Time.deltaTime * direction_facing), TRANSFORM.position.y - ((y_speed + speed_modifier) * Time.deltaTime), TRANSFORM.position.z);
    }
}
