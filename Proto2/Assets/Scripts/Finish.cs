using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] GameController Gc;

    private void OnTriggerEnter(Collider other)
    {
        Gc.Win();
    }
}
