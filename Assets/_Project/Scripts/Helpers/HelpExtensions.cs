public  static class HelpExtensions
{
    public static float RandomPickOne(float p_option1, float p_option2)
    {
        return UnityEngine.Random.Range(0, 2) == 0 ? p_option1 : p_option2;
    }
}
