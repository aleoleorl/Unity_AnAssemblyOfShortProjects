using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.src.Game;
using Assets.src.GameModule;

public class Main : MonoBehaviour
{
    private Manager3InRow manager;
    private Manager_3InRow manager2;

    // Start is called before the first frame update
    void Start()
    {
        manager = new Manager3InRow();
        manager2 = new Manager_3InRow();
    }

    // Update is called once per frame
    void Update()
    {
        manager2.Go();
    }
}
