using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string fadeInTrigger = "FadeIn";
    [SerializeField] private string fadeOutTrigger = "FadeOut";

    public void FadeIn()
    {
        GetComponent<Animator>().SetTrigger(fadeInTrigger);
    }
    public void FadeOut()
    {
        GetComponent<Animator>().SetTrigger(fadeOutTrigger);
    }
}