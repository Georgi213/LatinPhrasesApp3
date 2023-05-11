using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LatinPhrasesApp.Behaviors
{
    public class HeartButtonBehavior : Behavior<Button>
    {
        private Button _associatedButton;

        protected override void OnAttachedTo(Button button)
        {
            base.OnAttachedTo(button);
            _associatedButton = button;
            button.Pressed += HeartButton_Pressed;
            button.Released += HeartButton_Released;
        }

        protected override void OnDetachingFrom(Button button)
        {
            base.OnDetachingFrom(button);
            button.Pressed -= HeartButton_Pressed;
            button.Released -= HeartButton_Released;
            _associatedButton = null;
        }

        private void HeartButton_Pressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.TextColor = Color.DarkRed;
            }
        }

        private void HeartButton_Released(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.TextColor = Color.Default;
            }
        }
    }
}
