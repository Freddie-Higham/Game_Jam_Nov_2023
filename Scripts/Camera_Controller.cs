using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public float last_camera_movement_y;

    private Vector3 player_position;

    public float OFFSET;
    public float OFFSET_SMOOTHING;
    public float DISTANCE_TO_MOVE;
    public Vector3 CAMERA_STARTING_POSITION;
    
    public GameObject player;

    void Start()
    {
        last_camera_movement_y = player.transform.position.y;
        CAMERA_STARTING_POSITION = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        player_position = new Vector3(transform.position.x, player.transform.position.y + OFFSET, transform.position.z);

       // transform.position = new Vector3(transform.position.x, 40f, transform.position.z);
        //transform.position = CAMERA_STARTING_POSITION;
    }

    void Update()
    {
 
        //If the player has moved upwards the amount needed to justify moving the camera up.
        if (player.transform.position.y > last_camera_movement_y + DISTANCE_TO_MOVE) {
            player_position = new Vector3(transform.position.x, player.transform.position.y + OFFSET, transform.position.z);
            last_camera_movement_y = player.transform.position.y;
        //Same but checking if the camera needs moving downwards.
        }else if (player.transform.position.y < last_camera_movement_y - DISTANCE_TO_MOVE) {
            player_position = new Vector3(transform.position.x, player.transform.position.y - OFFSET, transform.position.z);
            last_camera_movement_y = player.transform.position.y;
        }else {
            player_position = new Vector3(player_position.x, player_position.y, player_position.z);
        }

/*         if (player_position.y < CAMERA_STARTING_POSITION.y) {
            player_position.y = CAMERA_STARTING_POSITION.y;
        } */

        transform.position = Vector3.Lerp(transform.position, player_position, OFFSET_SMOOTHING * Time.deltaTime);
    }
}
