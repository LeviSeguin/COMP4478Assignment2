using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //grid dimensions
    public int rows = 4;
    public int columns = 4;

    //array to assign random values to each card
    private int[] cardValues = new int[16];

    //number of cards flipped (either 0, 1, or 2)
    private int numCardsFlipped = 0;
    //number of matches
    private int numMatches = 0;
    private GameObject card1, card2;

    //holds card prefab
    public GameObject cardPrefab;
    //holds cards in scene
    public GameObject level;

    //array to hold references to cards
    GameObject[,] cardGrid;

    void Start()
    {
        //set card reference array size
        cardGrid = new GameObject[rows, columns];
        //get object to hold cards in scene
        level = GameObject.Find("Level");

        //initialize array with possible values
        for (int i = 0; i < cardValues.Length; i++)
        {
            cardValues[i] = (i / 2);
        }

        //shuffle array to randomize order of cards
        for (int i = 0; i < cardValues.Length - 1; i++)
        {
            int rnd = Random.Range(i, cardValues.Length);
            int temp = cardValues[rnd];
            cardValues[rnd] = cardValues[i];
            cardValues[i] = temp;
        }

        //fill array with cards
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                //instantiate prefab
                cardGrid[row, column] = Instantiate(cardPrefab);
                //get cardscript and set index
                CardScript cardScript = cardGrid[row, column].GetComponent<CardScript>();
                cardScript.setIndex(row, column);

                cardScript.setValue(cardValues[(row * columns) + column]);
                print(cardScript.getValue());

                //transform card
                cardGrid[row, column].transform.parent = level.transform;
                cardGrid[row, column].transform.position = new Vector3(column * 1.5f, row * 1.5f, 0);

            }
        }
        //center all cards
        level.transform.position = new Vector3(-(columns / 2) + 1, -(rows / 2), 0);

    }

    public void cardClicked(int row, int column)
    {
        StartCoroutine(waiter(row, column));
    }

    //card clicked logic
    private IEnumerator waiter(int row, int column)
    {
        GameObject card = cardGrid[row, column];
        CardScript cardScript = card.GetComponent<CardScript>();

        //if card is not flipped and not 2 cards already flipped
        if (!cardScript.getIsFlipped() && numCardsFlipped < 2)
        {
            cardScript.setIsFlipped(true);
            cardScript.setBackSprite();

            //if no cards flipped, store first card
            if (numCardsFlipped == 0)
            {
                numCardsFlipped = 1;
                card1 = cardGrid[row, column];
            
            }
            //if 1 card flipped, store second card and compare
            else if (numCardsFlipped == 1)
            {
                numCardsFlipped = 2;
                card2 = cardGrid[row, column];

                //get scripts
                CardScript card1Script = card1.GetComponent<CardScript>();
                CardScript card2Script = card2.GetComponent<CardScript>();

                //if cards match
                if (card1Script.getValue() == card2Script.getValue())
                {
                    numMatches++;
                    //check if win
                    if (numMatches >= 8) {
                        SceneManager.LoadScene("GameOver");
                    }
                
                }
                //if cards dont match
                else
                {
                    yield return new WaitForSeconds(1f);
                    card1Script.resetCard();
                    card2Script.resetCard();
                }

                numCardsFlipped = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
