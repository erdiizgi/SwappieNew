using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    GameObject loadingImage;

    void Start()
    {
        Resources.UnloadUnusedAssets();
        //loadingImage = GameObject.FindGameObjectWithTag("Loading");
        //loadingImage.SetActive(false);
    }

    public void LoadScene(int n)
    {
        //loadingImage.SetActive(true);
        Application.LoadLevel(n);
    }
}
