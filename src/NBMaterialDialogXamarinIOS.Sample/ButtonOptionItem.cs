using System;

namespace NBMaterialDialogXamarinIOS.Sample
{
    public class ButtonOptionItem
    {
        public ButtonOptionItem(string buttonCaption, string description)
        {
            if (buttonCaption == null)
            {
                throw new ArgumentNullException(nameof(buttonCaption));
            }

            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            ButtonCaption = buttonCaption;
            Description = description;
        }

        public string ButtonCaption { get; }

        public string Description { get; }

        public override string ToString()
        {
            return $"{nameof(ButtonCaption)}: {ButtonCaption}, {nameof(Description)}: {Description}";
        }
    }
}