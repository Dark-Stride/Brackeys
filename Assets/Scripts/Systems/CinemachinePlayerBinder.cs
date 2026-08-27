using UnityEngine;
#if UNITY_CINEMACHINE_V2
using Cinemachine;
#elif UNITY_CINEMACHINE_V3
using Unity.Cinemachine;
#endif

namespace Scripts.Systems
{
    public class CinemachinePlayerBinder : MonoBehaviour
    {
        [SerializeField] private string playerTag = "Player";

        private void OnEnable()
        {
            BindPlayerToCamera();
        }

        private void Start()
        {
            BindPlayerToCamera();
        }

        public void BindPlayerToCamera()
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player == null) return;

#if UNITY_CINEMACHINE_V2
            var vcam = GetComponent<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Follow = player.transform;
            }
#elif UNITY_CINEMACHINE_V3
            var vcam = GetComponent<CinemachineCamera>();
            if (vcam != null)
            {
                vcam.Target.TrackingTarget = player.transform;
            }
#else
            // Fallback via reflection to support any Cinemachine package version without compilation errors
            Component vcam = GetComponent("CinemachineVirtualCamera") ?? GetComponent("CinemachineCamera");
            if (vcam != null)
            {
                var followProp = vcam.GetType().GetProperty("Follow");
                if (followProp != null)
                {
                    followProp.SetValue(vcam, player.transform);
                }
            }
#endif
        }
    }
}
