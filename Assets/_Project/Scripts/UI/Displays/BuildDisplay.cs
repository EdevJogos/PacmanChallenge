using UnityEngine;

public class BuildDisplay : Display
{
    public TMPro.TextMeshProUGUI buildLabel;
    public TMPro.TextMeshProUGUI playerPosLabel;
    public TMPro.TextMeshProUGUI ghostPosLabel;
    public TMPro.TextMeshProUGUI buildTutLabel;
    public TMPro.TextMeshProUGUI feedbackLabel;

    public AudioSource clickSource;

    public override void UpdateDisplay(int p_operation)
    {
        switch (p_operation)
        {
            case 0:
                SetDefaultLabelColor();
                buildLabel.color = Color.green;
                buildTutLabel.color = Color.green;
                break;
            case 1:
                SetDefaultLabelColor();
                playerPosLabel.color = Color.green;
                break;
            case 2:
                SetDefaultLabelColor();
                ghostPosLabel.color = Color.green;
                break;
        }

        clickSource.Play();
    }

    public override void UpdateDisplay(string p_value)
    {
        feedbackLabel.text = p_value;
    }

    private void SetDefaultLabelColor()
    {
        buildLabel.color = Color.yellow;
        playerPosLabel.color = Color.yellow;
        ghostPosLabel.color = Color.yellow;
        buildTutLabel.color = Color.yellow;
    }
}
