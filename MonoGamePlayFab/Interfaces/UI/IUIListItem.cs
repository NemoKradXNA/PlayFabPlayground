using Microsoft.Xna.Framework;

namespace MonoGamePlayFab.Interfaces.UI
{
    public interface IUIListItem<T>
    {
        T Data { get; set; }
        Color TextColor { get; set; }
        string TextFormat { get; set; }

        string Text { get; }
        object Value { get; }

        string DisplayText { get; set; }
        string DisplayValue { get; set; }
    }
}
