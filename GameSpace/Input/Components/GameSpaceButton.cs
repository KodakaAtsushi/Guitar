using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class GameSpaceButton : MonoBehaviour, IPointerClickHandler
{
    public Observable<Unit> OnClick => onClick;
    Subject<Unit> onClick = new();

    Animator animator;
    AudioSource audioSource;

    float clickCoolTime = 0.11f;
    bool canClick = true;

    [SerializeField] Material disableMat;
    [SerializeField] AudioClip clickSound;

    CancellationToken ct => destroyCancellationToken;

    public void Init()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetDisable()
    {
        transform.position -= new Vector3(0, 0.1f, 0);
        var mr = GetComponent<MeshRenderer>();
        mr.material = disableMat;

        enabled = false;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnPinterClickHandler().Forget();
    }

    async UniTask OnPinterClickHandler()
    {
        if(!canClick) return;

        canClick = false;

        animator.Play("ButtonClick");
        audioSource.PlayOneShot(clickSound);
        onClick.OnNext(default);

        await UniTask.Delay((int)(clickCoolTime * 1000), cancellationToken:ct);

        canClick = true;
    }
}
