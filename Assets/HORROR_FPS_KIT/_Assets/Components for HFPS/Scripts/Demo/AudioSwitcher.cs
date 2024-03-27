using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitcher : MonoBehaviour {
    
    
//////////////////////////////////////
///
///     VALUES
///
///////////////////////////////////////
    
    
    [Header("User Options")]
    
    public bool startOnAwake;
    public bool autoHide;
    public float hideWait;
    
    [Space]
    
    public AudioSource source;
    public List<AudioClip> clips;
    public float clipBuff;
    
    [Header("Auto")]
    public int curClip;
    public bool locked;
    
    
//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////
    
    
    void Start() {
        
        curClip = -1;
        locked = false;
        
        if(startOnAwake){
            
            Clips_Loop();
            
        }//startOnAwake
        
    }//start
    
    
//////////////////////////////////////
///
///     CLIP ACTIONS
///
///////////////////////////////////////
    

    public void Clips_Loop(){
        
        StartCoroutine("ClipsLoop_Buff");
        
    }//Clips_Loop
    
    private IEnumerator ClipsLoop_Buff(){
        
        curClip += 1;
        
        if(curClip >= clips.Count){
            
            curClip = 0;
            
        }//curClip >= clips.Count
        
        source.PlayOneShot(clips[curClip]);
        
        yield return new WaitForSeconds(clips[curClip].length + clipBuff);
        
        Clips_Loop();
        
    }//ClipsLoop_Buff
    
    public void ClipsLoop_Stop(){
        
        locked = true;
        
        StopCoroutine("ClipsLoop_Buff");
        source.Stop();
        
        if(autoHide){
            
            StartCoroutine("HideBuff");
            
        }//autoHide
        
    }//ClipsLoop_Stop
    
    
//////////////////////////////////////
///
///     HIDE ACTIONS
///
///////////////////////////////////////
    
    
    private IEnumerator HideBuff(){
        
        yield return new WaitForSeconds(hideWait);
        
        this.gameObject.SetActive(false);
        
    }//HideBuff
    
    
}//AudioSwitcher
