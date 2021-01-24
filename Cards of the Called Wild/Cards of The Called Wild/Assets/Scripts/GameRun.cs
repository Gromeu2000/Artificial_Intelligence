using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRun : MonoBehaviour
{

	// Management of sprites
	private Object[] backgrounds;
	private Object[] props;
	private Object[] chars;

	// Game management
	private GameObject enemyCards;
	private int [] enemyChars;	
	private Agent agent;

    private GameObject playerCards;
    private int[] playerChars;
    private int[] characterSprite = { 4, 11, 15 }; //fox->4 frog->11 opossum->12

	private int NUM_ENEMY_CARDS = 3;
	private int NUM_CLASSES     = 3;
	private int DECK_SIZE       = 4;
    private int NUM_PLAYER_CARDS = 3;

    // Rewards
    private float RWD_ACTION_INVALID = -2.0f;
	private float RWD_HAND_LOST      = -1.0f;
	private float RWD_TIE            = -0.1f;
	private float RWD_HAND_WON       =  1.0f;

	// Other UI elements
	private UnityEngine.UI.Text textDeck;
    private UnityEngine.UI.Text textEnemyAction;
    private UnityEngine.UI.Text textPlayerAction;
    private UnityEngine.UI.Text textWinrate;

    private int countInvalid = 0;
    private int countLoss = 0;
    private int countTie = 0;
    private int countWin = 0;

    private UnityEngine.UI.Text textInvalid;
    private UnityEngine.UI.Text textLoss;
    private UnityEngine.UI.Text textTie;
    private UnityEngine.UI.Text textWin;

    // Start is called before the first frame update
    void Start()
    {


        ///////////////////////////////////////
        // Sprite management
        ///////////////////////////////////////

        // Load all prefabs
        backgrounds = Resources.LoadAll("Backgrounds/");
        props       = Resources.LoadAll("Props/");
        chars       = Resources.LoadAll("Chars/");


        ///////////////////////////////////////
        // UI management
        ///////////////////////////////////////
        textDeck = GameObject.Find("TextDeck").GetComponent<UnityEngine.UI.Text>();
        textEnemyAction = GameObject.Find("TextEnemyAction").GetComponent<UnityEngine.UI.Text>();
        textPlayerAction = GameObject.Find("TextPlayerAction").GetComponent<UnityEngine.UI.Text>();

        //Trackers
        //textInvalid = GameObject.Find("TextInvalid").GetComponent<UnityEngine.UI.Text>();
        textLoss = GameObject.Find("TextLosses").GetComponent<UnityEngine.UI.Text>();
        textTie = GameObject.Find("TextTies").GetComponent<UnityEngine.UI.Text>();
        textWin = GameObject.Find("TextWins").GetComponent<UnityEngine.UI.Text>();
        textWinrate = GameObject.Find("Winrate").GetComponent<UnityEngine.UI.Text>();


        ///////////////////////////////////////
        // Game management
        ///////////////////////////////////////
        enemyCards = GameObject.Find("EnemyCards");
        enemyChars = new int[NUM_ENEMY_CARDS];

        playerCards = GameObject.Find("PlayerCards");
        playerChars = new int[NUM_PLAYER_CARDS];


        agent = GameObject.Find("AgentManager").GetComponent<Agent>();

        agent.Initialize();


        ///////////////////////////////////////
        // Start the game
        ///////////////////////////////////////
        StartCoroutine("GenerateTurn");


        ///////////////////////////////////////
        // Image generation
        ///////////////////////////////////////
    	//renderTexture = gameObject.GetComponent<Camera>().targetTexture;

    	//imgWidth  = renderTexture.width;
    	//imgHeight = renderTexture.height;

        
    }


    // Generate a card on a given transform
    // Return the label (0-2) of the card
    private int GenerateCard(Transform parent)
    {

    	int idx = Random.Range(0, backgrounds.Length);
    	Instantiate(backgrounds[idx], parent.position, Quaternion.identity, parent);


    	idx               = Random.Range(0, props.Length);
    	Vector3 position = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), -1.0f);
   	  	Instantiate(props[idx], parent.position+position, Quaternion.identity, parent);

    	idx         = Random.Range(0, chars.Length);
    	position    = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), -2.0f);    	
   	  	Instantiate(chars[idx], parent.position+position, Quaternion.identity, parent);

   	  	// Determine label of the character, return it
   	  	int label = 0;
   	  	if(chars[idx].name.StartsWith("frog")) label = 1;
   	  	else if(chars[idx].name.StartsWith("opossum")) label = 2;

    	return label;
    } 

    // Generate another turn
    IEnumerator GenerateTurn()
    {
        for (int turn = 1; turn < 100000; turn++)
        {

            ///////////////////////////////////////
            // Generate enemy cards
            ///////////////////////////////////////

            // Destroy previous sprites (if any) and generate new cards
            int c = 0;
            foreach (Transform card in enemyCards.transform)
            {
                foreach (Transform sprite in card)
                {
                    Destroy(sprite.gameObject);
                }

                enemyChars[c++] = GenerateCard(card);
            }


            ///////////////////////////////////////
            // Generate player deck
            ///////////////////////////////////////
            int[] deck = GeneratePlayerDeck();
            textDeck.text = "Deck in turn: ";

            textDeck.text += turn.ToString();

            textDeck.text += "\n Action: ";

            foreach (int card in deck)
                textDeck.text += card.ToString() + "/";

        
            ///////////////////////////////////////
            // Tell the player to play
            ///////////////////////////////////////
            ///
            int[] action = agent.Play(deck, enemyChars);

            int b = 0;
            foreach (Transform card in playerCards.transform)
            {
                foreach (Transform sprite in card)
                {
                    Destroy(sprite.gameObject);
                }

                int idx = Random.Range(0, backgrounds.Length);
                Instantiate(backgrounds[idx], card.transform.position, Quaternion.identity, card.transform);
                Instantiate(chars[characterSprite[action[b]]], card.transform.position + new Vector3(0, 0, -1), Quaternion.identity, card.transform);
                b++;
            }


            textPlayerAction.text = "Action: ";
            foreach (int a in action)
            {
                textPlayerAction.text += a.ToString() + "/";
            }

            textEnemyAction.text = "Action: ";
            foreach (int a in enemyChars)
            {
                textEnemyAction.text += a.ToString() + "/";
            }

            textWinrate.text = "Winrate:";

            if (turn > 0)
            {
                float Wr = (float)countWin / (float)turn;
                Wr=Wr * 100;
                
                textWinrate.text += Wr.ToString()+"";
                
            }
          


            ///////////////////////////////////////
            // Compute reward
            ///////////////////////////////////////
            float reward = ComputeReward(deck, action);

            //Turns only count if not invalid
            /*if (reward != -2)
            {

                Debug.Log("Turn/reward: " + turn.ToString() + "->" + reward.ToString());
                turn++;
            }*/

            

            agent.GetReward(reward);

            switch (reward)
            {
                case -2.0f:
                    countInvalid++;
                    turn--;
                    textInvalid.text = "Invalids: " + countInvalid.ToString();
                    
                    break;
                case -1.0f:
                    countLoss++;
                    textLoss.text = "Losses: " + countLoss.ToString();
                    break;
                case -0.1f:
                    countTie++;
                    textTie.text = "Ties: " + countTie.ToString();
                    break;
                case 1.0f:
                    countWin++;
                    textWin.text = "Wins: " + countWin.ToString();
                    break;
                default:
                    break;
            }



            Debug.Log("Turn/reward: " + turn.ToString() + "->" + reward.ToString());


            // IMPORTANT: wait until the frame is rendered so the player sees
            //            the newly generated cards (otherwise it will see the previous ones)
            yield return new WaitForEndOfFrame();


            ///////////////////////////////////////
            // Manage turns/games
            ///////////////////////////////////////


            yield return new WaitForSeconds(0.1f);

    	}

    }


    // Auxiliary methods
    private int [] GeneratePlayerDeck()
    {
    	int [] deck = new int [DECK_SIZE];

    	for(int i=0; i<DECK_SIZE; i++)
    	{
    		deck[i] = Random.Range(0, NUM_CLASSES);  // high limit is exclusive so [0, NUM_CLASSES-1]
    	}

    	return deck;
    }

    // Compute the result of the turn and return the associated reward 
    // given the cards selected by the agent (action)
   	// deck -> array with the number of cards of each class the player has
   	// action -> array with the class of each card played
    private float ComputeReward(int [] deck, int [] action)
    {
        int[] tmpDeck = deck;
        bool invalid = true;
        // First check if the action is valid given the player's deck
        foreach (int card in action)
        {
            invalid = true;
            for (int i = 0; i < tmpDeck.Length; i++)
            {
                if (tmpDeck[i] == card)
                {
                    tmpDeck[i] = -1;
                    invalid = false;
                    break;
                }
            }

            if (invalid)
            {
                
                return RWD_ACTION_INVALID;
            }
        }



            // Second see who wins
            int score = 0;
    	for(int i=0; i<NUM_ENEMY_CARDS; i++)
    	{
    		if(action[i] != enemyChars[i])
    		{
    			if(action[i] > enemyChars[i] || action[i]==0 && enemyChars[i]==2)	
    				score++;
    			else
    				score--;
    		}
    		
    	}

    	if(score == 0) return RWD_TIE;
    	else if(score > 0) return RWD_HAND_WON;
    	else return RWD_HAND_LOST;
    }
}
