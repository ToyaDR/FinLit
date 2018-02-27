using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient {

    private string ing_name;
    private int ing_price;
	private Sprite ing_sprite;

    public void addName(string name) {
        ing_name = name;
    }

    public void addPrice(int price) {
        ing_price = price;
    }

	public void addSprite(string sprite) {
		ing_sprite = Resources.Load (sprite, typeof(Sprite)) as Sprite;
	}

    public string getName() {
        return ing_name;
    }

    public int getPrice() {
        return ing_price;
    }

	public Sprite getSprite() {
		return ing_sprite;
	}
}
