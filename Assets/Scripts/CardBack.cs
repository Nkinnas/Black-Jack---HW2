using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBackManager : MonoBehaviour
{
    public static CardBackManager instance; 

    public Sprite redCardBackSprite; 
    public Sprite blueCardBackSprite; 

    private Sprite chosenCardBackSprite; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

  
    public void SetRedCardBack()
    {
        chosenCardBackSprite = redCardBackSprite;
    }

  
    public void SetBlueCardBack()
    {
        chosenCardBackSprite = blueCardBackSprite;
    }

 
    public Sprite GetCardBackSprite()
    {
        return chosenCardBackSprite;
    }
}
