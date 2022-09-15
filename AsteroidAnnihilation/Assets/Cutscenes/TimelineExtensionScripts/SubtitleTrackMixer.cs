using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class SubtitleTrackMixer : PlayableBehaviour
{

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TMP_Text text = playerData as TMP_Text;

        string currentText = "";
        float currentAlpha = 0f;

        if(!text)
        {
            return;
        }

        int inputCount = playable.GetInputCount();

        for(int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);

            if(inputWeight > 0f)
            {
                ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);

                SubtitleBehaviour input = inputPlayable.GetBehaviour();
                currentText = input.SubtitleText;
                currentAlpha = inputWeight;
            }
        }

        text.text = currentText;
        text.color = new Color(1, 1, 1, currentAlpha);
    }
}

