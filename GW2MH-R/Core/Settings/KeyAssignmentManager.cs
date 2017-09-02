using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GW2MH.Core.Settings
{
    internal class KeyAssignmentManager
    {

        public Dictionary<Button, KeyEventArgs> ButtonList { get; private set; }
        public Button CurrentButtonToAssign { get; private set; }

        public KeyAssignmentManager()
        {
            ButtonList = new Dictionary<Button, KeyEventArgs>();
        }

        public void RegisterButton(Button button)
        {
            if (!ButtonList.ContainsKey(button))
            {
                ButtonList.Add(button, null);
                button.Click += Button_Click;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            CurrentButtonToAssign = button;

            button.Text = "<Press Key>";
        }

        public void KeyUpTriggered(KeyEventArgs keyEventArgs)
        {
            if (CurrentButtonToAssign == null) return;
            if (keyEventArgs.KeyCode == Keys.Escape)
            {
                CurrentButtonToAssign.Text = ButtonList[CurrentButtonToAssign]?.Modifiers == Keys.None ? ButtonList[CurrentButtonToAssign]?.KeyCode.ToString() : ButtonList[CurrentButtonToAssign]?.Modifiers.ToString() + " + " + ButtonList[CurrentButtonToAssign]?.KeyCode.ToString();
                CurrentButtonToAssign = null;
            }
            else
            {
                ButtonList[CurrentButtonToAssign] = keyEventArgs;
                CurrentButtonToAssign.Text = keyEventArgs.Modifiers == Keys.None ? keyEventArgs.KeyCode.ToString() : keyEventArgs.Modifiers.ToString() + " + " + keyEventArgs.KeyCode.ToString();
                CurrentButtonToAssign = null;
            }

            SettingsManager.Save();
        }
    }
}