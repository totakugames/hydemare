using UnityEngine;

public class Exec_Chest : ObjectExecution
{
  [SerializeField]
  private GameObject spawnPrefab;

  [SerializeField] 
  private StoryPanel storyPanel;

  [SerializeField]
  private string successStory;

  public override void execute() 
  {
    if (!string.IsNullOrEmpty(successStory) && storyPanel != null)
    {
        storyPanel.ShowStory(successStory, transform);
    }

    GameObject spawn = Instantiate(
      spawnPrefab,
      transform.position,
      spawnPrefab.transform.rotation
    );

    Debug.Log("chest puzzle was solved, wee!");
  }
}
