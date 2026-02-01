using UnityEngine;

interface IObjectExecution 
{
  void execute();
}

public class ObjectExecution : MonoBehaviour, IObjectExecution 
{
  public virtual void execute() {}
}

