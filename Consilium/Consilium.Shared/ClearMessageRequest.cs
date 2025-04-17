using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared;
public class ClearMessagesRequest : CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<bool> {
    public ClearMessagesRequest() : base(true) { }
}