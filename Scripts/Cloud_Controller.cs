using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Controller : MonoBehaviour
{
    private Transform TRANSFORM;
    private SpriteRenderer RENDERER;
    private float distance_travelled;
    private float RETURN_POSITION_X;
    [SerializeField] private float MAX_DISTANCE_X;
    public float speed;
    private bool triggered;
    private float transparency;

    [SerializeField] private AudioSource CLOUD_BREAK_SOUND;

    void Start() {
        TRANSFORM = GetComponent<Transform>();
        RETURN_POSITION_X = TRANSFORM.position.x;
        triggered = false;
        transparency = 0.6f;
        RENDERER = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Arrow") {
            CLOUD_BREAK_SOUND.Play();
        }
        if (collision.gameObject.tag == "Arrow" || collision.gameObject.tag == "Player") {
            Destroy(gameObject, 0.3f);
            triggered = true;
        }
    }

    void Update() {
        distance_travelled = RETURN_POSITION_X - TRANSFORM.position.x;
        TRANSFORM.position = new Vector3(TRANSFORM.position.x - (speed * Time.deltaTime), TRANSFORM.position.y, TRANSFORM.position.z);
        if (distance_travelled > MAX_DISTANCE_X) {
            TRANSFORM.position = new Vector3(RETURN_POSITION_X, TRANSFORM.position.y, TRANSFORM.position.z);
        }

        if (triggered) {
            transparency -= 0.003f;
            RENDERER.color = new Color(1f, 1f, 1f, transparency); 
        }
    }
}
