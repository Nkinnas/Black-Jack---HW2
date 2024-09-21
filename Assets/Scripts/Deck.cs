using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Deck : MonoBehaviour
{
    public GameObject[] cardPrefabsRed; 
    public GameObject[] cardPrefabsBlue; 

    public GameObject[] cardPrefabsGray;
    public Button dealButton; 
    public Button backToMenuButton;
    public Animator gameOverAnimator;
    public TextMeshProUGUI gameOutcomeText;
    

    public TextMeshProUGUI playerScoreText;  
    public TextMeshProUGUI dealerScoreText;  

    public GameObject redDeck; 
    public GameObject blueDeck; 

    public GameObject grayDeck;
    private GameObject currentVisibleDeck;  


    private GameObject[] activeCardPrefabs; 

    int[] cardValues = new int[52];
    int currentCardIndex = 0;

    List<GameObject> playerCards = new List<GameObject>();
    List<GameObject> dealerCards = new List<GameObject>();
    int playerTotalValue = 0;
    int dealerTotalValue = 0;

    GameObject dealerFaceDownCard; 

    bool isGameOver = false;
    bool initialHandDealt = false;

    void Start()
    {
        AssignCardValues();
        SetCardBackToBlue();
        string cardColor = PlayerPrefs.GetString("SelectedCardColor", "Red"); 
        if (cardColor == "Red")
        {
            SetCardBackToRed();
        }
        else if (cardColor == "Blue")
        {
            SetCardBackToBlue();
        }
        else if (cardColor == "Gray")
        {
            SetCardBackToGray();
        }
        ShuffleDeck();
        dealButton.gameObject.SetActive(true);
        backToMenuButton.gameObject.SetActive(false);
        gameOutcomeText.gameObject.SetActive(false);
        gameOverAnimator.gameObject.SetActive(false);
    }


    public void SetCardBackToRed()
    {
        activeCardPrefabs = cardPrefabsRed;
        redDeck.SetActive(true);
        blueDeck.SetActive(false);
        grayDeck.SetActive(false);
        currentVisibleDeck = redDeck;

    }


    public void SetCardBackToBlue()
    {
        redDeck.SetActive(false);
        blueDeck.SetActive(true);
        grayDeck.SetActive(false);
        currentVisibleDeck = blueDeck;
 
        activeCardPrefabs = cardPrefabsBlue;

    }

    public void SetCardBackToGray()
    {
    activeCardPrefabs = cardPrefabsGray;
    redDeck.SetActive(false);
    blueDeck.SetActive(false);
    grayDeck.SetActive(true);
    currentVisibleDeck = grayDeck;
    }

    void AssignCardValues()
    {
        for (int i = 0; i < cardValues.Length; i++)
        {
            int cardRank = i % 13;
            if (cardRank == 0) cardValues[i] = 1; 
            else if (cardRank >= 10) cardValues[i] = 10; 
            else cardValues[i] = cardRank + 1; 
        }
    }

    void ShuffleDeck()
    {

        for (int i = cardValues.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int tempValue = cardValues[i];
            cardValues[i] = cardValues[randomIndex];
            cardValues[randomIndex] = tempValue;

            GameObject tempPrefab = activeCardPrefabs[i];
            activeCardPrefabs[i] = activeCardPrefabs[randomIndex];
            activeCardPrefabs[randomIndex] = tempPrefab;
        }
    
    }

    public void DealCard(Vector3 basePosition, float offsetX, bool isFaceDown, List<GameObject> hand, ref int totalValue)
    {
        if (currentCardIndex < activeCardPrefabs.Length)
        {
            GameObject card = Instantiate(activeCardPrefabs[currentCardIndex], new Vector3(basePosition.x + offsetX, basePosition.y, basePosition.z), Quaternion.identity);
            hand.Add(card);

            Card cardComponent = card.GetComponent<Card>();
            if (cardComponent != null)
            {
                cardComponent.value = cardValues[currentCardIndex];
                totalValue += cardComponent.value;
            }

            if (!isFaceDown)
            {
                card.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                dealerFaceDownCard = card;
            }
            currentCardIndex++;
        }
    
    }

    public void DealInitialHands()
    {
        Vector3 playerBasePosition = new Vector3(-2, -3, 0);
        Vector3 dealerBasePosition = new Vector3(-2, 3, 0);
        float cardSpacing = 1.0f;

        DealCard(playerBasePosition, 0, false, playerCards, ref playerTotalValue);
        DealCard(playerBasePosition, cardSpacing, false, playerCards, ref playerTotalValue);

        DealCard(dealerBasePosition, 0, true, dealerCards, ref dealerTotalValue); 
        DealCard(dealerBasePosition, cardSpacing, false, dealerCards, ref dealerTotalValue); 

        dealButton.gameObject.SetActive(false);

        initialHandDealt = true;
    }

    void DetermineGameOutcome()
    {
        if (dealerTotalValue > 21)
        {
            EndGame("Player Wins! Dealer Busts!");
        }
        else if (dealerTotalValue > playerTotalValue)
        {
            EndGame("Dealer Wins!");
        }
        else if (playerTotalValue > dealerTotalValue)
        {
            EndGame("Player Wins!");
        }
        else
        {
            EndGame("It's a Tie!");
        }
    }

    void PlayerHit()
    {
        if (!initialHandDealt || isGameOver) return;

        Vector3 playerBasePosition = new Vector3(-2, -3, 0);
        float cardSpacing = 1.0f;

        DealCard(playerBasePosition, playerCards.Count * cardSpacing, false, playerCards, ref playerTotalValue);
 

        if (playerTotalValue > 21)
        {

            DealerPlay();
            EndGame("Dealer Wins! Player Busts!");
        }
    }

    public void PlayerStay()
    {
        if (!initialHandDealt || isGameOver) return;

    
        DealerPlay();
    }

    void DealerPlay()
    {
        Vector3 dealerBasePosition = new Vector3(-2, 3, 0);
        float cardSpacing = 1.0f;

        dealerFaceDownCard.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        while (dealerTotalValue < 17)
        {
            DealCard(dealerBasePosition, dealerCards.Count * cardSpacing, false, dealerCards, ref dealerTotalValue);
        }

        DetermineGameOutcome();
    }

void EndGame(string resultMessage)
{
    isGameOver = true;
    dealButton.gameObject.SetActive(false);
    gameOverAnimator.gameObject.SetActive(true);
    gameOverAnimator.SetTrigger("ShowGameOver");
    backToMenuButton.gameObject.SetActive(true);

 
    gameOutcomeText.gameObject.SetActive(true);
    gameOutcomeText.text = resultMessage;


    playerScoreText.gameObject.SetActive(true);
    dealerScoreText.gameObject.SetActive(true);

    string playerScoreMessage = $"Player Score: {playerTotalValue}";
    if (playerTotalValue > 21)
    {
        playerScoreMessage += " (Bust)";
    }

    string dealerScoreMessage = $"Dealer Score: {dealerTotalValue}";
    if (dealerTotalValue > 21)
    {
        dealerScoreMessage += " (Bust)";
    }

    playerScoreText.text = playerScoreMessage;
    dealerScoreText.text = dealerScoreMessage;
}


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MAINMENUSCENE");
    }
}
