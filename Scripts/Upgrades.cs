using System.Collections;
using System.Collections.Generic;

public static class Upgrades {

    public static bool[,] upgrades_status = {{true, true, true}, {true, true, true}};
    //public static bool[,] upgrades_status = {{false, false, false}, {false, false, false}};

    public static bool[,] upgrades_selected = {{true, false, false}, {false, false, false}};

    public static int[,] upgrades_costs = {{1, 2, 3}, {1, 2, 3}};

    public static string[,] upgrades_details = {{"Your dashes move further.", "Your dashes recharge faster and you can hold one more dash charge.", "You now dash with a sword in your hand. Allows you to break clouds with your dash."},
    {"Your arrows move faster.", "Your arrows recharge faster and you can hold one more arrow charge.", "Hitting obstacles with your arrow has a 50% chance to recharge your dash."}};
}