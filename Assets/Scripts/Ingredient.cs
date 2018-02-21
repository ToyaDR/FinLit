using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient {

    public string ing_name;
    public int ing_price;

    public void addName(string name) {
        ing_name = name;
    }

    public void addPrice(int price) {
        ing_price = price;
    }

    public string getName() {
        return ing_name;
    }

    public int getPrice() {
        return ing_price;
    }
}
