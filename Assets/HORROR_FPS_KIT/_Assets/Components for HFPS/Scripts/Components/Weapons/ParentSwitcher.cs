using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentSwitcher : MonoBehaviour {
    
    
    public Transform holder;
    
    public bool setZero;
    public bool setOrigSize;
    
    public Vector3 origSize;

    void Start() {
        
        setZero = false;
        
    }//start

    
    public void Parent_Switch(Transform newParent){
        
        if(holder != null){
           
            holder.parent = newParent;
            
            if(setZero){
                
                holder.localPosition = new Vector3(0, 0, 0);
                holder.rotation = new Quaternion(0, 0, 0, 0);
                
                setZero = false;
                
            }//setZero
            
            if(setOrigSize){
                
                holder.localScale = origSize;
                
                setOrigSize = false;
                
            }//setOrigSize
            
        }//holder != null
        
    }//Parent_Switch
    
    public void SetZero_State(bool state){
        
        setZero = state;
        
    }//SetZero_State
    
    public void OrigSize_Catch(){
       
        origSize = holder.localScale;
        
    }//OrigSize_Catch
    
    public void SetOrigSize_State(bool state){
        
        setOrigSize = state;
        
    }//SetOrigSize_State
    
}
