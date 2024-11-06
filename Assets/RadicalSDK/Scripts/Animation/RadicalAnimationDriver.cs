using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maps the animation data from the stream onto any humanoid character.
/// </summary>
namespace Radical
{
    public class RadicalAnimationDriver : RadicalCharacterAnimator
    {
        public Animator sourceAnimator, targetAnimator;
        [SerializeField] HumanPoseHandler sourcePoseHandler, targetPoseHandler;

        protected override void Awake()
        {
            //sourceAnimator = GetComponent<Animator>();
            nextFrameData.hasBeenProcessed = true; // don't start trying to process before the 1. frame has been received
            nextFaceData.hasBeenProcessed = true;
        }

        public void Init(GameObject characterPrefab)
        {
            //print("Creating driver for: " + characterPrefab.name + " from: " + name);
            isNativeCharacter = true;
            if (characterPrefab.TryGetComponent(out targetAnimator))
            {
                BindSkeletton();
                sourceAnimator = GetComponent<Animator>();
                Avatar sourceAvatar = sourceAnimator.avatar; // The avatar of the driver
                Avatar targetAvatar = targetAnimator.avatar; // The avatar of the custom character
                if (targetAvatar == null)
                {
                    Debug.LogError(characterPrefab.name + " was not set up in humanoid mode.");
                    return;
                }

                sourcePoseHandler = new HumanPoseHandler(sourceAvatar, transform);
                targetPoseHandler = new HumanPoseHandler(targetAvatar, targetAnimator.transform);
                //print($"{name}: assigning pose handlers");
            }
            else
                Debug.LogError(characterPrefab.name + " Has no Animator component.");
            
            tryStoreBlendShapeMatrix(characterPrefab);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
           
            HumanPose pose = new HumanPose();
            sourcePoseHandler.GetHumanPose(ref pose);
            targetPoseHandler.SetHumanPose(ref pose);
        }
    }
}
