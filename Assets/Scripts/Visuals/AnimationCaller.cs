using System.Collections;
using UnityEngine;

public class AnimationCaller : MonoBehaviour
{
    [SerializeField] private string _triggerName;
    [SerializeField] private Vector2 _timeBetweenCycles = new Vector2(5, 10);

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (true)
        {
            float waittime = UnityEngine.Random.Range(_timeBetweenCycles.x, _timeBetweenCycles.y);
            _animator.SetTrigger(_triggerName);
            yield return new WaitForSeconds(waittime);
        }
    }
}
