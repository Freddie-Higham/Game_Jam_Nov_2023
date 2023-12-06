using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Controller : MonoBehaviour
{
    private Transform TRANSFORM;
    private float distance_travelled;
    private float RETURN_POSITION_X;
    [SerializeField] private float MAX_POSITION_X;
    public float x_speed;
    public float y_speed;
    public float y_distance;
    private float y_counter;
    public float direction_facing;

    void Start() {
        y_counter = 0;
        TRANSFORM = GetComponent<Transform>();
        distance_travelled = 0;
        RETURN_POSITION_X = TRANSFORM.position.x;
    }

    void Update() {
        distance_travelled += x_speed * Time.deltaTime * direction_facing;
        y_counter += y_speed * Time.deltaTime;

        if (y_counter >= y_distance || y_counter <= -y_distance) {
            y_speed = -y_speed;
            y_counter = 0;
        }

        TRANSFORM.position = new Vector3(TRANSFORM.position.x - (x_speed * Time.deltaTime * -direction_facing), TRANSFORM.position.y + (y_speed * Time.deltaTime), TRANSFORM.position.z);
        if ((TRANSFORM.position.x < MAX_POSITION_X && direction_facing == -1) || (TRANSFORM.position.x > MAX_POSITION_X && direction_facing == 1)) {
            distance_travelled = 0;
            TRANSFORM.position = new Vector3(RETURN_POSITION_X, TRANSFORM.position.y, TRANSFORM.position.z);
        }
    }
}
