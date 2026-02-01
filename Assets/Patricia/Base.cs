using UnityEngine;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField]
    private List<string> neededItems = new List<string>();
    [SerializeField]
    private ObjectExecution executedFunction;

    public void test() {
        executedFunction.execute();
    }

    // todo auswertungslogik
    // item entweder "annehmen" oder droppen
    // dann exec ausführen, sobald alle benötigten items da sind 
}
