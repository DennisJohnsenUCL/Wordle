namespace Wordle_WinForms.Controllers
{
    public class NavigationController<TEnum> where TEnum : Enum
    {
        private readonly Dictionary<TEnum, UserControl> _viewContainer = [];

        public NavigationController() { }

        public bool Register(TEnum key, UserControl view)
        {
            if (view != null)
            {
                return _viewContainer.TryAdd(key, view);
            }
            throw new ArgumentNullException(nameof(view));
        }

        public bool Unregister(TEnum key)
        {
            return _viewContainer.Remove(key);
        }

        public void NavigateTo(TEnum key)
        {
            if (_viewContainer.TryGetValue(key, out var view) && _viewContainer[key] != null)
            {
                view.Visible = true;
                view.Focus();
                view.BringToFront();

                foreach (var viewEntry in _viewContainer)
                {
                    if (!viewEntry.Key.Equals(key) && viewEntry.Value != null)
                    {
                        viewEntry.Value.Visible = false;
                    }
                }
            }
            else throw new ArgumentNullException(nameof(key));
        }
    }
}
