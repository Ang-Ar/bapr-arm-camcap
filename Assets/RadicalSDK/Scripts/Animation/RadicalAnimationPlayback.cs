using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

namespace Radical
{
    /// <summary>
    /// Loads an AnimationPlaybackObject from the project and animates a character from it.
    /// </summary>
    //[RequireComponent(typeof(RadicalCharacterAnimator))]
    public class RadicalAnimationPlayback : MonoBehaviour
    {
        #region Public Members
        public AnimationPlaybackObject playbackObject;
        //public Skinned
        RadicalCharacterAnimator characterAnimator;
        public bool playOnStart = true;
        public bool loop = true;
        public int startFrame = 0;
        public int endFrame = 10000;
        //public float framesPerSecond = 30f;
        #endregion

        #region Private Members
        bool isPlaying;
        bool useFace;
        int currentFrame;
        BakedFrameData[] bakedFrames;
        #endregion

        #region Public Accessors
        public void StartPlayback()
        {
            if (playbackObject == null)
            {
                print("There is no animation object assigned to " + name);
                return;
            }
            //else
            //    print("Starting at frame: " + bakedFrames[0].frame);
            isPlaying = true;
            
            //StartCoroutine(PersistentPlayback());
        }
        public void StopPlayback()
        {
            isPlaying = false;
        }
        #endregion

        #region Functionality
        void Start()
        {
            //print("Recorded frames: " + playbackObject.animation.Length);
            if (startFrame < 0 || startFrame > playbackObject.animation.Length) startFrame = 0;
            
            if (TryGetComponent(out characterAnimator))
            {
                useFace = true;
                //isNativeCharacter = true; //It's an armature following the radical rig naming standards
            }
            else
            {
                string playerID = playbackObject.playerID; //It's a custom character, so we need a driver for the playback
                //print("Setting up driver for " + name);
                useFace = playbackObject.hasFace;
                print("Using face: " + useFace);
                characterAnimator = RadicalPlayerManager.CreateAnimationDriver(playerID, gameObject);
                //print("Created: " + characterAnimator.name);
                (characterAnimator as RadicalAnimationDriver).Init(gameObject);
            }
            bakedFrames = playbackObject.animation;

            if (playOnStart) // Call StartPlayback() or StopPlayback() from anywhere to start/stop animation
                StartCoroutine(WaitForBindingComplete());
            //TODO: Verify face rig
        }

        IEnumerator WaitForBindingComplete()
        {
            // the skeletton needs to be initialized for any character to start playback.
            yield return new WaitForEndOfFrame();
            currentFrame = startFrame;
            StartPlayback();
        }

        //private void FixedUpdate()
        private void Update()
        {
            if (!isPlaying) return;
            BakedFrameData frameData = bakedFrames[currentFrame];
            characterAnimator.SetPose(frameData);
            //FeetStabilizer.m_instance.Stabilize(frameData);
            try
            {
                if (useFace)
                {
                    BakedFaceData bakedFaceData = playbackObject.facialAnimation[currentFrame];
                    characterAnimator.SetFacialExpression(bakedFaceData);
                }
            }
            catch { }
            currentFrame++;
            int mod = bakedFrames.Length < endFrame ? bakedFrames.Length - 2 : endFrame;
            if (currentFrame > mod)
            {
                if (loop)
                {
                    currentFrame = startFrame;
                }
                else
                {
                    isPlaying = false;
                }
            }
        }

        //IEnumerator PersistentPlayback()
        //{
        //    BakedFrameData[] animation = playbackObject.animation;
        //    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f / framesPerSecond);
        //    isPlaying = true;
        //    currentFrame = startFrame;
        //    Stopwatch sw = new Stopwatch();

        //    while (isPlaying)
        //    {
        //        sw.Start();
        //        // Read one frame every 1/30s and apply the pose
        //        BakedFrameData frameData = animation[currentFrame];
        //        characterAnimator.SetPose(frameData);
        //        if (useFace)
        //        {
        //            BakedFaceData bakedFaceData = playbackObject.facialAnimation[currentFrame];
        //            characterAnimator.SetFacialExpression(bakedFaceData);
        //        }

        //        if (loop)
        //            currentFrame = (currentFrame + 1) % animation.Length;
        //        else
        //        {
        //            currentFrame++;
        //            if (currentFrame == animation.Length)
        //               yield break;
        //        }

        //        yield return wait;
        //        sw.Stop();
        //        print("Elapsed: " + sw.ElapsedMilliseconds);
        //        sw.Reset();
        //    }
        //}
        #endregion
    }
}