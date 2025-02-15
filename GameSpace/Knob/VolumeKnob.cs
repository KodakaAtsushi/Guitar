using UnityEngine;
using R3;

public class VolumeKnob : MonoBehaviour, IDraggable
{
    [SerializeField] AudioGroup audioGroup;
    Vector3 parentPos;

    public bool CanDrag => true;

    float lineLengthZ = 4;
    float knobScaleZ;
    float maxPosZ;

    public Observable<(AudioGroup audioGroup, float volume)> OnValueChange => onValueChange;
    Subject<(AudioGroup audioGroup, float volume)> onValueChange = new();

    public void Init()
    {
        knobScaleZ = transform.localScale.z;
        maxPosZ = lineLengthZ/2 - knobScaleZ/2 + 0.01f; // knobの端とlineの端を合わせる lineがはみ出ないようにちょっと足す

        parentPos = transform.parent.position;
    }

    void IDraggable.OnPickHandler() // 何もしないintafaceの実装は良くない？
    {
        return;
    }

    void IDraggable.OnDragHandler(Vector3 worldPos)
    {
        var localPos = worldPos - parentPos;
        var clampedPosZ = Mathf.Clamp(localPos.z, -maxPosZ, maxPosZ);
        transform.localPosition = new Vector3(0, 0, clampedPosZ);

        var volume = GetSliderValue();
        onValueChange.OnNext((audioGroup, volume));
    }

    void IDraggable.OnDropHandler()
    {
        return;
    }

    void IDraggable.RetrunParent()
    {
        return;
    }

    // line上の相対的な位置　1-0の間
    float GetSliderValue()
    {
        return (transform.localPosition.z + maxPosZ) / (2 * maxPosZ);
    }
}
