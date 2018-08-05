# TimelineTool

You can change Unity Timeline clips realtime very easy by TimelineTool.cs:
```
public PlayableDirector playableDirector;
public CinemachineVirtualCameraBase vCam1;
public AnimationClip CubeAni1;
    
void Start()
{
    TimelineTool timeline = new TimelineTool(playableDirector);
    timeline.SetCinemachineClips("Cinemachine Track", "vCam", vCam1);
    timeline.SetAnimationClip("Cube Track", "AniClip", CubeAni1);
}
```

Set/Get PlayableDirector Bindings:
```
public PlayableDirector playableDirector;

void Start()
{
    timeline = new TimelineTool(playableDirector);
    timeline.GetBinding<GameObject>("Activation Track");
    timeline.SetBinding("Activation Track",gameObject);
}
```

Get clip's duration:
```
public PlayableDirector playableDirector;

void Start()
{
    timeline = new TimelineTool(playableDirector);
    float duration = (float)timeline.GetClip("Cinemachine Track", "vCam").duration;
}
```

1. Please keep the names of all tracks unique!
2. For all cinemachine clips you want to change, you need set Vitural Camera "Unexposed".
3. All function based on the name of clip and track.
