using WGestures.Core.Persistence;

namespace WGestures.Core.Commands
{
    internal interface IGestureIntentStoreAware {
        IGestureIntentStore IntentStore { set; }
    }
}