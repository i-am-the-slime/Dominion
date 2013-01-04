using UnityEngine;
using System.Collections;

public delegate void GUICallback(Object[] results);
public class GUIManager : MonoBehaviour
{
    public static ModalDialogue modalDialogue = null;
    public Player player;
    private GUISkin _skin;
    private GUIMode _mode;
    public Game game;
    
    public void ChangeMode(GUIMode mode)
    {
        Debug.Log("Changing mode to " + mode + ".");
        _mode.FinishMode();
        _mode = mode;
    }
    private Card hoveredCard=null;
	// Use this for initialization
	void Start()
	{
        _skin = Resources.Load("GUI/DominionGUISkin") as GUISkin;
	    _mode = new WatchMode(player);
	}
	
	// Update is called once per frame
	void Update () {
        // Right-click and hold on card shows it in the center of the screen.
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 50.0f)) {
                Card card = hit.collider.gameObject.GetComponent<Card>();
                if(card!=null){
                    hoveredCard = card;
                    card.ShowCardInfo = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(1) && hoveredCard!=null) {
            hoveredCard.ShowCardInfo = false;
            hoveredCard = null;
        }

	}

    void OnGUI()
    {
        GUI.skin = _skin;
		GUI.Label (new Rect(10, 10, 400, 230), _mode.ModeName);
        switch (_mode.ModeName)
        {
            case "CardSelection":
                if (GUI.Button(new Rect(Screen.width - 105, Screen.height - 40, 100, 35), "Done"))
                {
                    Debug.Log("Done button clicked in selection mode.");
                    ChangeMode(new ActionMode(player));
                }
                break;
            case "Action":
                if (GUI.Button(new Rect(Screen.width - 105, Screen.height - 40, 100, 35), "Buy cards"))
                {
					Debug.Log ("Switching to buy mode.");
                    ChangeMode(new BuyMode(player));
                }
                break;
            case "Buy":
                if (GUI.Button(new Rect(Screen.width - 105, Screen.height - 40, 100, 35), "End turn"))
                {
					Debug.Log("Beginning new Action mode for next player");
                    ChangeMode(new ActionMode(player.leftNeighbour));
                    player = player.leftNeighbour;
                    game.cam.ChangeViewTo(player);
                } 
                break;
            case "Watch":
                break;
            default:
                Debug.LogError("Invalid mode: " + _mode.ModeName);
                break;
        }

       if (modalDialogue!=null)
       {
           GUI.Box(new Rect(Screen.width/2 -150, Screen.height/2 -75, 300, 150), modalDialogue.Question);
           if(GUI.Button(new Rect(Screen.width/2 -145, Screen.height/2 +35, 140, 30), "Yes"))
           {
               modalDialogue.YesCallback(null);
               modalDialogue = null;
           }

           if (GUI.Button(new Rect(Screen.width/2 + 5, Screen.height/2 + 35, 140, 30), "No")) 
           {
               modalDialogue.NoCallback(null);
               modalDialogue = null;
           }
           
       }
        //General info
        string stats = "Actions: " + player.Actions + "    Money: " + player.Money + "   Buys: " + player.Buys;
        GUI.Box(new Rect(5, Screen.height - 40, Screen.width -115, 35), stats);
    }
}
