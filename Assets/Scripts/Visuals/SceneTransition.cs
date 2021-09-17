using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string _fadeInTrigger = "FadeIn";
    [SerializeField] private string _fadeOutTrigger = "FadeOut";
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        _animator.SetTrigger(_fadeInTrigger);
    }
    public void FadeOut()
    {
        _animator.SetTrigger(_fadeOutTrigger);
    }
}