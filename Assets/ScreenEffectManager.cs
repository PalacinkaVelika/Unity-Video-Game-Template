using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScreenEffectManager : MonoBehaviour {
    public static ScreenEffectManager Instance { get; private set; }
    public NoiseSettings sixDShakeProfile;
    Camera m_camera;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            m_camera = FindAnyObjectByType<Camera>();
        } else {
            Destroy(gameObject);
        }
    }

    public void ScreenShakeImpulse(float strength, float frequency, float duration) {
        CinemachineBrain cmBrain = m_camera.GetComponent<CinemachineBrain>();
        ICinemachineCamera vcam = cmBrain.ActiveVirtualCamera;
        CinemachineVirtualCamera cineVcam = vcam as CinemachineVirtualCamera;
        if (cineVcam != null) {
            // Get or add the noise component
            CinemachineBasicMultiChannelPerlin noise = cineVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise == null) {
                noise = cineVcam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            noise.enabled = true;
            noise.m_NoiseProfile = sixDShakeProfile;
            noise.m_AmplitudeGain = strength;
            noise.m_FrequencyGain = frequency;
            StartCoroutine(RemoveNoiseAfterDuration(noise, duration));
        }
    }

    private IEnumerator RemoveNoiseAfterDuration(CinemachineBasicMultiChannelPerlin noise, float duration) {
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
        noise.enabled = false;
    }
}
