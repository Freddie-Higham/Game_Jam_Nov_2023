using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    private Transform TRANSFORM;
     private SpriteRenderer RENDERER;
    public int direction_facing;
    public float x_speed;
    public float y_speed;

    public void setPosition(Vector3 pos, int d) {
        TRANSFORM.position = pos;
        if (d != 0) {
            direction_facing = d;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RENDERER = GetComponent<SpriteRenderer>();
        TRANSFORM = GetComponent<Transform>();
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

        TRANSFORM.position = new Vector3(TRANSFORM.position.x + (x_speed * Time.deltaTime * direction_facing), TRANSFORM.position.y - (y_speed * Time.deltaTime), TRANSFORM.position.z);
    }
}
