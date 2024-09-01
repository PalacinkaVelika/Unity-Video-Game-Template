using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : UIBehaviour {

    public override void Hide() {
        UtilityUI.Fade(canvas, false, .5f);
    }

    public override void Show() {
        UtilityUI.Fade(canvas, true, .5f);
    }
}
