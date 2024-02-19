using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Accept_Button : MonoBehaviour
{
    private SpriteRenderer RENDERER;

    public Upgrades_Menu_Controller UPGRADES_SCRIPT;

    public bool locked;

    void Start()
    {
        RENDERER = GetComponent<SpriteRenderer>();
        locked = false;
    }

    public void setLocked(bool state) {
        locked = state;
    }

    public void OnMouseUp() {

        if (!locked) {
            UPGRADES_SCRIPT.acceptPressed();
        }

        //RENDERER.color = new Color(22f/255f, 217f/255f, 45f/255f, 80f);
    
    }

    void Update()
    {
        if (locked) {
            RENDERER.enabled = false;
        }
        else {
            RENDERER.enabled = true;
        }
    }
}