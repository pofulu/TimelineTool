using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using Cinemachine.Timeline;
using Cinemachine;

public class TimelineTool
{
    private Dictionary<string, PlayableBinding> bindingDict;
    private Dictionary<TimelineClip, string> clipNameDict;
    private PlayableDirector playableDirector;
    public bool IsModified
    {
        get
        {
            return bindingDict.Count > 0;
        }
    }

    public TimelineTool(PlayableDirector playableDirector)
    {
        this.playableDirector = playableDirector;
        ConstructBindingDict();
        ConstructClipNameDict();
    }

    public void ConstructBindingDict()
    {
        bindingDict = new Dictionary<string, PlayableBinding>();

        foreach (var at in playableDirector.playableAsset.outputs)
        {
            if (!bindingDict.ContainsKey(at.streamName))
            {
                bindingDict.Add(at.streamName, at);
            }
        }
    }

    public void ConstructClipNameDict()
    {
        clipNameDict = new Dictionary<TimelineClip, string>();

        foreach (var track in GetAllTracks())
        {
            foreach (var clip in GetAllClips(track.streamName))
            {
                clipNameDict.Add(clip, clip.displayName);
            }
        }
    }

    //-----------------------------------------------------------------

    public void Apply(bool playAfterApplied = true)
    {
        // Cinemachine's clip name will be change after RebuildGraph().
        if (IsModified)
        {
            playableDirector.RebuildGraph();
            ApplyModifiedClipsName();
        }

        if (playAfterApplied)
        {
            playableDirector.Play();
        }
    }

    private void ApplyModifiedClipsName()
    {
        foreach (var dict in clipNameDict)
        {
            dict.Key.displayName = dict.Value;
        }
    }

    private void ModifyClipName(TimelineClip modifiedClip, string clipName)
    {
        clipNameDict[modifiedClip] = clipName;
    }

    //-----------------------------------------------------------------

    public PlayableBinding[] GetAllTracks()
    {
        return bindingDict.Values.ToArray();
    }

    public PlayableBinding GetTrack(string trackName)
    {
        try
        {
            return bindingDict[trackName];
        }
        catch (System.Exception)
        {
            throw new System.Exception(string.Format("Track '{0}' is not exist.", trackName));
        }
    }

    public TrackType GetBinding<TrackType>(string trackName) where TrackType : Object
    {
        return GetTrack(trackName).sourceObject as TrackType;
    }

    public Object GetKey(string trackName)
    {
        return GetBinding<Object>(trackName);
    }

    public void SetBinding(string trackName, Object binding)
    {
        playableDirector.SetGenericBinding(GetKey(trackName), binding);
    }

    //-----------------------------------------------------------------

    public TimelineClip GetClip(string trackName, string clipName)
    {
        try
        {
            return GetClips(trackName, clipName)[0];
        }
        catch (System.Exception)
        {
            throw new System.Exception(string.Format("There's no '{0}' clip on '{1}'.", clipName, trackName));
        }
    }

    public TimelineClip[] GetClips(string trackName, string clipName)
    {
        var track = GetBinding<TrackAsset>(trackName);
        var clips = from info in track.GetClips()
                    where info.displayName == clipName
                    select info;
        return clips.ToArray();
    }

    public TimelineClip[] GetAllClips(string trackName)
    {
        var track = GetBinding<TrackAsset>(trackName);
        return track.GetClips().ToArray();
    }

    //----------------------------------------------------------------

    public ClipAssetType GetClipAsset<ClipAssetType>(string trackName, string clipName) where ClipAssetType : Object
    {
        var clip = GetClip(trackName, clipName);
        ClipAssetType asset = clip.asset as ClipAssetType;
        ModifyClipName(clip, clipName);
        return asset;
    }

    public ClipAssetType[] GetClipsAsset<ClipAssetType>(string trackName, string clipName) where ClipAssetType : Object
    {
        var clips = GetClips(trackName, clipName);
        var assets = new ClipAssetType[clips.Length];
        for (int i = 0; i < clips.Length; i++)
        {
            assets[i] = clips[i].asset as ClipAssetType;
            ModifyClipName(clips[i], clipName);
        }
        return assets;
    }

    //----------------------------------------------------------------

    public void SetCinemachineClip(string trackName, string clipName, CinemachineVirtualCameraBase virtualCamera)
    {
        GetClipAsset<CinemachineShot>(trackName, clipName).VirtualCamera.defaultValue = virtualCamera;
    }

    public void SetCinemachineClips(string trackName, string clipName, CinemachineVirtualCameraBase virtualCamera)
    {
        foreach (var clip in GetClipsAsset<CinemachineShot>(trackName, clipName))
        {
            clip.VirtualCamera.defaultValue = virtualCamera;
        }
    }

    public void SetAnimationClip(string trackName, string clipName, AnimationClip animationClip)
    {
        GetClipAsset<AnimationPlayableAsset>(trackName, clipName).clip = animationClip;
    }

    public void SetAnimationClips(string trackName, string clipName, AnimationClip animationClip)
    {
        foreach (var clip in GetClipsAsset<AnimationPlayableAsset>(trackName, clipName))
        {
            clip.clip = animationClip;
        }
    }
}
