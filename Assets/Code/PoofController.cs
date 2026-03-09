using UnityEngine;

public class PoofController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Animate()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().Play(0);
    }

    public void AnimationDone()
    {
        gameObject.SetActive(false);
    }
}
