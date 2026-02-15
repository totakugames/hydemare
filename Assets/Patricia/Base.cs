using UnityEngine;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField]
    public string objectName;
    [SerializeField]
    public List<string> neededItems = new List<string>();
    [SerializeField]
    private ObjectExecution executedFunction;

    private List<string> remainingItems;

    void Awake()
    {
        remainingItems = new List<string>(neededItems);
    }

    public void ConsumeItem(string itemName) 
    {
        if (remainingItems.Contains(itemName)) {

            remainingItems.Remove(itemName);

            if (remainingItems.Count == 0)
            {
                AllItemsDelivered();
            }
        }
        else {
            // todo textbox
        }
        Debug.Log(remainingItems.Count + " item(s) missing.");
    }

    private void AllItemsDelivered()
    {
        Debug.Log("aayyyyy");
        executedFunction.execute();
    }
}
