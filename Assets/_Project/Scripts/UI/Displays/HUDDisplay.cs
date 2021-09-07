

public class HUDDisplay : Display
{
    public TMPro.TextMeshProUGUI levelLabel;
    public TMPro.TextMeshProUGUI pointsLabel;
    public TMPro.TextMeshProUGUI lifessLabel;

    public override void UpdateDisplay(int p_operation, int p_value)
    {
        switch (p_operation)
        {
            case 0:
                pointsLabel.text = p_value + "";
                break;
            case 1:
                lifessLabel.text = p_value + "";
                break;
            case 2:
                levelLabel.text = p_value + "";
                break;
        }
    }
}
