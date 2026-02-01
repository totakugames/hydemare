using UnityEngine;

interface IObjectExecution 
{
  void execute();
}

public class ObjectExecution : MonoBehaviour, IObjectExecution 
{
  public void execute() {}
}

