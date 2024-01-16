using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void toTitle()
    {
        SceneManager.LoadScene("Menu");
    }

    public void again()
    {
        SceneManager.LoadScene("Main");
    }
}
