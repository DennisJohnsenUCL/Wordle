namespace Wordle_WinForms.Interfaces
{
    public interface INavigationController<TEnum> where TEnum : Enum
    {
        bool Register(TEnum key, UserControl view);
        bool Unregister(TEnum key);
        void NavigateTo(TEnum Key);
    }
}
