using UnityEngine;

public class Exec_Chest : ObjectExecution
{
  [SerializeField]
  private GameObject spawnPrefab;

  public override void execute() 
  {
    // todo textbox

        GameObject spawn = Instantiate(
          spawnPrefab,
          transform.position,
          spawnPrefab.transform.rotation
        );

    Debug.Log("chest puzzle was solved, wee!");
  }
}
