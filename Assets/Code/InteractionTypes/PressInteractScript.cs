using UnityEngine;
using UnityEngine.Events;

public class ButtonController : Interactable
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform targetPos;
    [SerializeField] private Transform button;
    [SerializeField] private Vector3 offsetAfterPress;
    private Vector3 startPos;
    public HoldingState state;
    private EntityAudioManager audioManager;
    public UnityEvent OnPress;
    public UnityEvent OnRelease;

    private void Start()
    {
        startPos = button.position;
        audioManager = GetComponent<EntityAudioManager>();
        GetMeshRenderers();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(button.position, targetPos.position);

        switch (state)
        {
            case HoldingState.Pressing:
                if (distance > 0.01f)
                {
                    button.position = Vector3.MoveTowards(button.position, targetPos.position, speed * Time.fixedDeltaTime);
                }
                else
                {
                    state = HoldingState.setPressed;
                    PlaySound();
                    OnPress.Invoke();
                }
                break;

            case HoldingState.Pressed:
                button.position = Vector3.MoveTowards(button.position, targetPos.position + offsetAfterPress, speed * Time.fixedDeltaTime);
                break;

            case HoldingState.Releasing:
                if (distance > 0.01f)
                {
                    button.position = Vector3.MoveTowards(button.position, targetPos.position, speed * Time.fixedDeltaTime);
                }
                else
                {
                    state = HoldingState.setRelease;
                    PlaySound();
                    OnRelease.Invoke();
                }
                break;

            case HoldingState.Released:
                button.position = Vector3.MoveTowards(button.position, startPos, speed * Time.fixedDeltaTime);
                break;
        }

    }
    protected override void StartInteract()
    {
        switch (state)
        {
            case HoldingState.Released:
                state = HoldingState.Pressing;
                break;
            case HoldingState.Pressed:
                state = HoldingState.Releasing;
                break;
        }
    }

    protected override void StopInteract()
    {
        switch (state)
        {
            case HoldingState.Pressed:
                state = HoldingState.Releasing;
                break;
            case HoldingState.setPressed:
                state = HoldingState.Pressed;
                PlaySound();
                break;
            case HoldingState.setRelease:
                state = HoldingState.Released;
                PlaySound();
                break;
            case HoldingState.Pressing:
                state = HoldingState.Releasing;
                break;
        }
    }
    private void PlaySound()
    {
        audioManager.PlayGunshot();
    }
}

public enum HoldingState
{
    Pressed, Released, Pressing, Releasing, setPressed, setRelease
}