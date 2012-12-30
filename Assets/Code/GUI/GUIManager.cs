using UnityEngine;
using System.Collections;

public delegate void GUICallback(Object[] results);

public class GUIManager : MonoBehaviour
{
    public static ModalDialogue modalDialogue = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI()
    {
       if (modalDialogue!=null)
       {
           GUI.Box(new Rect(Screen.width/2 -150, Screen.height/2 -75, 300, 150), modalDialogue.Question);
           if(GUI.Button(new Rect(Screen.width/2 -145, Screen.height/2 +35, 140, 30), "Yes"))
           {
               modalDialogue.YesCallback(null);
               modalDialogue = null;
           }

           if(GUI.Button(new Rect(Screen.width/2 +5, Screen.height/2 +35, 140, 30), "No"))
           {
               modalDialogue.NoCallback(null);
               modalDialogue = null;
           }
           
       }
    }
}
