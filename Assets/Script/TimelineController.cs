using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public CinemachineVirtualCameraBase vCam1;
    public CinemachineVirtualCameraBase vCam2;
    public AnimationClip CubeAni1;
    public AnimationClip CubeAni2;

    private TimelineTool timeline;

    void Start()
    {
        timeline = new TimelineTool(playableDirector);
    }

    public void Demo1()
    {
        timeline.SetCinemachineClips("Cinemachine Track", "vCam", vCam1);
        timeline.Apply();
    }

    public void Demo2()
    {
        timeline.SetCinemachineClips("Cinemachine Track", "vCam", vCam2);
        timeline.Apply();
    }

    public void Demo3()
    {
        timeline.SetAnimationClip("Cube Track", "AniClip", CubeAni1);
        timeline.Apply();
    }

    public void Demo4()
    {
        timeline.SetAnimationClip("Cube Track", "AniClip", CubeAni2);
        timeline.Apply();
    }
}
