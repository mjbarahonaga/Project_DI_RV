using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Utad.XRInteractionSystem.Input
{
	/// <summary>
	/// Esta clase gestiona la activación de cualquier acción con un controlador
	/// </summary>
	public class ControllerButtonListener : MonoBehaviour
	{
		[SerializeField] private InputActionReference _actionReference;

		[Space]
		public UnityEvent _onActionPerformed;
		public UnityEvent _onActionCancelled;

		private void OnEnable()
		{
			_actionReference.action.performed += HandleOnActionPerformed;
			_actionReference.action.canceled += HandleOnActionCancelled;
		}
		
		private void OnDisable()
		{
			_actionReference.action.performed -= HandleOnActionPerformed;
			_actionReference.action.canceled -= HandleOnActionCancelled;
		}
		
		private void HandleOnActionPerformed(InputAction.CallbackContext obj)
		{
			_onActionPerformed.Invoke();	
		}

		private void HandleOnActionCancelled(InputAction.CallbackContext obj)
		{
			_onActionCancelled.Invoke();
		}
	}
}