using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {
    public int SceneBack;
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Escape)){
            if (SceneBack != -1)
                Application.LoadLevel(this.SceneBack);
            else
                Application.Quit();
        }
	}
}
