using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Consilium.Maui;

public class ThemeChangedMessage : ValueChangedMessage<string> {
    public ThemeChangedMessage(string value) : base(value) { }
}
