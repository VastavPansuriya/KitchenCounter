using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipsRefSO audioClipsRefSO;

    
    public static SoundManager Instance { get; private set; }

    private const string PLAYERPREF_SOUND_EFFECT_VOLUME = "SoundEffectsVolume";
    private float volume = 1f;
    private void Awake()
    {
        volume = PlayerPrefs.GetFloat(PLAYERPREF_SOUND_EFFECT_VOLUME, 1);
        Instance = this;
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
        PlateKitchenObject.OnPlateUsed += PlateKitchenObject_OnPlateUsed;
    }

    private void PlateKitchenObject_OnPlateUsed(object sender, EventArgs e)
    {
        Debug.Log("Working " + sender);
        PlateKitchenObject trashCounter = (PlateKitchenObject)sender;
        PlaySound(audioClipsRefSO.objectPickup, trashCounter.transform.position);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        Debug.Log("Working " + sender);
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(audioClipsRefSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        Debug.Log("Working " + sender);

        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioClipsRefSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickSomething(object sender, EventArgs e)
    {
        Debug.Log("Working " + sender);

        PlaySound(audioClipsRefSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        Debug.Log("Working " + sender);

        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipsRefSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        Debug.Log("Working " + sender);

        PlaySound(audioClipsRefSO.deliveryFailed, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        Debug.Log("Working " + sender);

        PlaySound(audioClipsRefSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeSuccess -= DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed -= DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut -= CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSomething -= Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedHere -= BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;
        PlateKitchenObject.OnPlateUsed -= PlateKitchenObject_OnPlateUsed;
    }
    public void PlaySound(AudioClip[] audioClip, Vector3 pos, float volume = 1)
    {
        AudioClip current = audioClip[UnityEngine.Random.Range(0, audioClip.Length)];
        PlaySound(current, pos, volume);
    }
    public void PlaySound(AudioClip audioClip, Vector3 pos, float volumeMultiplier = 1)
    {
        AudioSource.PlayClipAtPoint(audioClip, pos, volumeMultiplier * volume);
    }

    public void PlayFootStepsSound(Vector3 pos, float volume)
    {
        PlaySound(audioClipsRefSO.footSteps, pos, volume);
    }

    public void PlayCountDownSound()
    {
        PlaySound(audioClipsRefSO.warning, Vector3.zero, volume);
    }
    public void PlayWarningSound(Vector3 pos)
    {
        PlaySound(audioClipsRefSO.warning, pos, volume);
    }
    public void ChangeVolume()
    {
        volume += .1f;

        if (volume > 1f)
        {
            volume = 0;
        }
        PlayerPrefs.SetFloat(PLAYERPREF_SOUND_EFFECT_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
