using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] GameController Gc;

    private void OnTriggerEnter(Collider other)
    {
        //Gc.Win();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
