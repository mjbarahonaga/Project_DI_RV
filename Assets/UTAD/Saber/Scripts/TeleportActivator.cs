using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Utad.XRInteractionToolkit.Interaction.Input
{
	/// <summary>
	/// Esta clase gestiona la activación del XRController del teleport que tiene como hijo.
	/// </summary>
	public class TeleportActivator : MonoBehaviour
	{
		[SerializeField] private XRRayInteractor _teleportRayInteractor;
		private ActionBasedController _teleportActionBasedController;

		private void Awake()
		{
			_teleportActionBasedController = _teleportRayInteractor.GetComponent<ActionBasedController>();
		}
		
		public void ActivateTeleport()
		{
			TeleportControllerEnabled(true);
		}

		public void DeactivateTeleport()
		{
			Invoke(nameof(WaitAndDeactivate), 0.1f);
		}

		private void WaitAndDeactivate()
		{
			TeleportControllerEnabled(false);
		}

		private void TeleportControllerEnabled(bool value)
		{
			_teleportRayInteractor.enabled = value;
			_teleportActionBasedController.enableInputActions = value;
		}
	}
}