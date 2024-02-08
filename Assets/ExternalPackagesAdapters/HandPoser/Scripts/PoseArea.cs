using UnityEngine;
using UnityEngine.Events;

namespace LudusXR.ExternalAssetsReferences.HandPoser.Scripts
{
	/// <summary>
	/// Changes hand pose when it enters this collider
	/// </summary>
	public class PoseArea : MonoBehaviour
	{
		[SerializeField] private Pose _pose;

		[SerializeField] private UnityEvent _onHandEnter;
		[SerializeField] private UnityEvent _onHandExit;
		
		private void OnTriggerEnter(Collider other)
		{
			var handWithPose = other.GetComponentInChildren<GameplayHand>();
			if (handWithPose == null) return;
			handWithPose.ApplyPose(_pose);
			_onHandEnter.Invoke();
		}

		private void OnTriggerExit(Collider other)
		{
			var handWithPose = other.GetComponentInChildren<GameplayHand>();
			if (handWithPose == null) return;
			
			handWithPose.ApplyDefaultPose();
			_onHandExit.Invoke();
		}
	}
}