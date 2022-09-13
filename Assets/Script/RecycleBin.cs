using Game.Player;

namespace Game
{
    public class RecycleBin : ReceivableObject
    {
        public override bool AcceptObject(ThrowableObject throwableObject) =>
            throwableObject.GetComponent<ResourceObject>() != null;

        protected override void HandleReceive(PlayerInteractControl interactor)
        {
            ResourceObject resourceObject = interactor.pickedObject.GetComponent<ResourceObject>();
            if (resourceObject == null) return;
            interactor.SubmitObject();
            resourceObject.ReturnToPool();
        }
    }
}