using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Upgrades_Menu_Controller : MonoBehaviour
{

    public TextMeshProUGUI GOLD_COUNTER;

    public TextMeshProUGUI GOLD_COST;

    public TextMeshProUGUI UPGRADE_DETAILS;

    private int current_index_1;
    private int current_index_2;

/*     public GameObject dash_upgrade_1;
    public GameObject dash_upgrade_2;
    public GameObject dash_upgrade_3;
    public GameObject arrow_upgrade_1;
    public GameObject arrow_upgrade_2;
    public GameObject arrow_upgrade_3; */

    // Start is called before the first frame update
    void Start()
    {
        current_index_1 = 0;
        current_index_2 = 0;
    }

    public void acceptPressed() 
    {
        Debug.Log("Accept Pressed.");

        if (Upgrades.upgrades_costs[current_index_1, current_index_2] <= Gold.gold) {
            Gold.gold -= Upgrades.upgrades_costs[current_index_1, current_index_2];
            Upgrades.upgrades_status[current_index_1, current_index_2] = true;
            Debug.Log("Upgrade made");
        }
        else {
            Debug.Log("Can't afford upgrade");
        }
    }

    public void upgradePressed(int index1, int index2) 
    {
        for (int i_1 = 0; i_1 < 2; i_1++) {
            for (int i_2 = 0; i_2 < 3; i_2++) {
                Upgrades.upgrades_selected[i_1, i_2] = false;
            }
        }

        //Upgrades.upgrades_selected[upgrade_type, upgrade_index] = {{false, false, false}, {false, false, false}};

        current_index_1 = index1;
        current_index_2 = index2;

        Upgrades.upgrades_selected[index1, index2] = true;
        UPGRADE_DETAILS.text = Upgrades.upgrades_details[index1, index2];
        GOLD_COST.text = $"Cost: {Upgrades.upgrades_costs[index1, index2]} Gold";
    }

    // Update is called once per frame
    void Update()
    {
        GOLD_COUNTER.text = $"x{Gold.gold}";
    }
}