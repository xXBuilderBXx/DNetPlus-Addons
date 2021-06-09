using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNetPlus_InteractiveButtons
{
    public class InteractiveButtonData
    {
        public InteractionData Interaction;
        public IUser User;

        public void Update(InteractionData data, IUser user)
        {
            Interaction = data;
            User = user;
        }
    }
}
