using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    //value to determine which back sprite to display
    private int cardValue = 0;
    //used to determine behaviour when a card is clicked
    private bool isFlipped = false;
    //stores position in grid
    private int row;
    private int column;

    //holds sprites for front and back
    private Sprite frontSprite;
    private Sprite[] backSprite; 
     
    private SpriteRenderer rend;

    //holds game manager script
    GameManager gameManager; 


    // Start is called before the first frame update
    void Start()
    {
        //get game manager script
        gameManager = GameObject.Find("Scripts").GetComponent<GameManager>();
        //setup renderer and frontsprite
        rend = GetComponent<SpriteRenderer>();
        frontSprite = Resources.Load<Sprite>("Sprites/blank"); 

        //load all possible back sprites
        backSprite = Resources.LoadAll<Sprite>("Sprites/FlippedCard");

        //render the front sprite
        rend.sprite = frontSprite;
        print(rend.bounds.size);
    }

    //on click, flip a card if it is not already flipped
    void OnMouseDown() {
        gameManager.cardClicked(row, column);

    }

    public void setIndex(int row, int column) {
        this.row = row;
        this.column = column;
    }

    public bool getIsFlipped() {
        return isFlipped;
    }

    public void setIsFlipped(bool b) {
        isFlipped = b;
    }

    public void setValue(int value) {
        cardValue = value;
    }

    public int getValue() {
        return cardValue;
    }

    public void setBackSprite() {
        rend.sprite = backSprite[cardValue];
    }

    //reset a card to not flipped
    public void resetCard() {
        print("card reset");
        isFlipped = false;
        rend.sprite = frontSprite;
    }
}
