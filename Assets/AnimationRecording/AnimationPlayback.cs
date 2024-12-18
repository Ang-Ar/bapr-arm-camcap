using UnityEngine;

public class AnimationPlayback : MonoBehaviour
{
    // NOTE: also look into animator recorder & playback functionality!

    public Animator Target { get => target; }
    public bool IsActive { get; private set; }

    [SerializeField] private Animator target;
    [SerializeField] private AnimationClip clip;
    [SerializeField] private RuntimeAnimatorController basePlaybackController;

    RuntimeAnimatorController savedController;
    Avatar savedAvatar;
    AnimatorOverrideController playbackController;

    public void Start()
    {
        playbackController = new AnimatorOverrideController(basePlaybackController);
        playbackController["Playback Animation"] = clip;
    }

    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;

        savedController = Target.runtimeAnimatorController;
        target.runtimeAnimatorController = playbackController;

        savedAvatar = target.avatar;
        if (!clip.isHumanMotion)
        {
            target.avatar = null;
        }
    }

    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;

        target.runtimeAnimatorController = savedController;
        target.avatar = savedAvatar;
    }
}
