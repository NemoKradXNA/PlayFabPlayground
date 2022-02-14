using Microsoft.Xna.Framework;
using MonoGamePlayFab.Interfaces.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MonoGamePlayFab.UI
{
    public class UIListItem<T> : IUIListItem<T>
    {
        protected string _text;

        public T Data { get; set; }
        public Color TextColor { get; set; }
        public string TextFormat { get; set; }

        public string DisplayText { get; set; }
        public string DisplayValue { get; set; }

        protected object _value;
        public object Value
        {
            get
            {
                if (_value == null)
                {
                    if (string.IsNullOrEmpty(DisplayValue))
                        _value = 0;
                    else
                    {
                        PropertyInfo prop = Data.GetType().GetProperty(DisplayValue);
                        if (prop == null)
                            _value = 0;
                        else
                        {
                            _value = prop.GetValue(Data);
                        }
                    }
                }

                return _text;
            }
        }

        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(_text))
                {
                    if (string.IsNullOrEmpty(DisplayText))
                        _text = Data.ToString();
                    else
                    {
                        PropertyInfo prop = Data.GetType().GetProperty(DisplayText);
                        if (prop == null)
                            _text = Data.ToString();
                        else
                        {
                            _text = prop.GetValue(Data).ToString();
                        }
                    }                    
                }

                return _text;
            }
        }
    }
}
