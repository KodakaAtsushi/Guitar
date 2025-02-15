using System.Collections.Generic;
using System.Linq;
using R3;

public class ArrowController
{
    public ArrowController(Arrow arrow, IEnumerable<SoundListener> soundListeners)
    {
        foreach(var sl in soundListeners)
        {
            sl.OnEnable.Subscribe(_ => {
                arrow.MoveTo(sl.transform.position);
            }).AddTo(sl);
        }

        var firstSoundListener = soundListeners.ElementAt(0);
        arrow.MoveTo(firstSoundListener.transform.position);
    }
}
