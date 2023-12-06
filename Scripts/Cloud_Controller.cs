using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Controller : MonoBehaviour
{
    private Transform TRANSFORM;
    private float distance_travelled;
    private float RETURN_POSITION_X;
    [SerializeField] private float MAX_POSITION_X;
    public float speed;

    void Start() {
        TRANSFORM = GetComponent<Transform>();
        distance_travelled = 0;
        RETURN_POSITION_X = TRANSFORM.position.x;
    }

    void Update() {
        distance_travelled += 0.001f;
        TRANSFORM.position = new Vector3(TRANSFORM.position.x - (speed * Time.deltaTime), TRANSFORM.position.y, TRANSFORM.position.z);
        if (TRANSFORM.position.x < MAX_POSITION_X) {
            distance_travelled = 0;
            TRANSFORM.position = new Vector3(RETURN_POSITION_X, TRANSFORM.position.y, TRANSFORM.position.z);
        }
    }
}
