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

    [SerializeField] 
    private StoryPanel storyPanel;
    [SerializeField]
    private string partialStory;
    [SerializeField]
    public string failureStory;

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
            else {
                if (!string.IsNullOrEmpty(partialStory) && storyPanel != null)
                {
                    storyPanel.ShowStory(partialStory, transform);
                }
            }
        }

        Debug.Log(remainingItems.Count + " item(s) missing.");
    }

    private void AllItemsDelivered()
    {
        Debug.Log("aayyyyy");
        executedFunction.execute();
    }
}
