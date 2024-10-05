namespace Assets.Scripts.Abstracts.Inputs
{
    internal interface IInput
    {
        bool GetButtonDown0 { get; }
        bool GetButtonUp0 { get; }
        bool GetButton0 { get; }
        bool GetButtonDown1 { get; }
        bool GetButtonUp1 { get; }
        bool GetButton1 { get; }
        bool GetKeyDownEscape { get; }
    }
}
