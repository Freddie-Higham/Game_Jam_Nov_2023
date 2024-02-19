using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Upgrade_Button : MonoBehaviour
{

    public int upgrade_type;
    public int upgrade_index;

    private SpriteRenderer RENDERER;

    public Upgrades_Menu_Controller UPGRADES_SCRIPT;

    private bool unlocked;

    public Accept_Button ACCEPT_BUTTON_SCRIPT;

    void Start()
    {
        RENDERER = GetComponent<SpriteRenderer>();
        unlocked = true;
    }

    public void OnMouseUp() {

        UPGRADES_SCRIPT.upgradePressed(upgrade_type, upgrade_index);

        //RENDERER.color = new Color(22f/255f, 217f/255f, 45f/255f, 80f);
    
    }

    void Update()
    {

        if (Upgrades.upgrades_status[upgrade_type, upgrade_index] == true) {
            RENDERER.color = new Color(51f/255f, 78f/255f, 195f/255f, 80f);
            unlocked = false;
        }
        else{
            bool possible = true;
            for (int i=0; i<upgrade_index; i++) {
                if (Upgrades.upgrades_status[upgrade_type, i] == false) {
                    possible = false;
                }
            }
           // Debug.Log($"({!possible} || {Upgrades.upgrades_costs[upgrade_type, upgrade_index]} <= {Gold.gold})");
            if (!possible || Upgrades.upgrades_costs[upgrade_type, upgrade_index] > Gold.gold) {
                unlocked = false;
                RENDERER.color = new Color(188f/255f, 0f/255f, 0f/255f, 118f/255f);
            }
            else {
                unlocked = true;
                RENDERER.color = new Color(22f/255f, 217f/255f, 45f/255f, 118f/255f);
            }
        }

        if (Upgrades.upgrades_selected[upgrade_type, upgrade_index] == true) {
            RENDERER.color = new Color(RENDERER.color.r, RENDERER.color.g, RENDERER.color.b, 150f/255f);
            if (unlocked) {
                ACCEPT_BUTTON_SCRIPT.setLocked(false);
            }
            else {
                ACCEPT_BUTTON_SCRIPT.setLocked(true);
            }
        }
        else {
            RENDERER.color = new Color(RENDERER.color.r, RENDERER.color.g, RENDERER.color.b, 108f/255f);
        }

    }
}