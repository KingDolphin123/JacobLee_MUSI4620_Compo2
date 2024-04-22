using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int beat;
    public LibPdInstance pdPatch;
    [SerializeField] bool oneOverOne;
    [SerializeField] bool threeOverFour;
    [SerializeField] bool sevenOverEight;
    [SerializeField] bool fiveOverFour;
    [SerializeField] bool oneOverTwo;
    [SerializeField] bool threeOverEight;
    [SerializeField] bool oneOverFour;
    [SerializeField] bool threeOverSixteen;
    List<float> timeArr = new List<float>();
    List<float> sigArr = new List<float>();
    // bool tick = true;
    string[] sound_type = new string[] { 
        "oneOverOne", 
        "threeOverFour", 
        "sevenOverEight", 
        "fiveOverFour",
        "oneOverTwo",
        "threeOverEight",
        "oneOverFour",
        "threeOverSixteen",
        };
    bool[] soundList;
    public List<AudioClip> sounds;
    GameObject[] soundObjs;
    Renderer[] soundObjsRenderers;
    void Start()
    {
        soundObjs = new GameObject[sounds.Count];
        soundObjsRenderers =  new Renderer[sounds.Count];
        Application.targetFrameRate = 60;
        for (int i = 0; i < sounds.Count; i++)
        {
            soundObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            soundObjsRenderers[i] = soundObjs[i].GetComponent<Renderer>();
            soundObjsRenderers[i].enabled = false;
            Color color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            soundObjsRenderers[i].material.color = color;
            string name = sounds[i].name + ".wav";
            pdPatch.SendSymbol(sound_type[i], name);
            string[] temp = sounds[i].name.Split('-');
            float timeSig = (float)Convert.ToInt32(temp[0])/(float)Convert.ToInt32(temp[1]);
            Debug.Log(timeSig);
            sigArr.Add(timeSig);
            timeArr.Add(0);
        }
    }
    public int getBeat(){
        return beat;
    }
    private void incrementTime(){
        float time = (int)(Time.smoothDeltaTime *1000) ;

        // Debug.Log((int)(1.0f / Time.smoothDeltaTime));
        Debug.Log(time + " " + "hi");
        for (int i = 0; i < timeArr.Count; i++){
            timeArr[i] += time;
        }
    }
    private void resetSoundList(){
        soundList = new bool[] {
            oneOverOne, 
            threeOverFour, 
            sevenOverEight,
            fiveOverFour,
            oneOverTwo,
            threeOverEight,
            oneOverFour,
            threeOverSixteen
            };
    }
    void Update()
    {
        resetSoundList();
        incrementTime();
        for (int i = 0; i < timeArr.Count; i++){
            if(timeArr[i] > beat * sigArr[i]){       
                if (soundList[i]){
                    // Debug.Log(beat * sigArr[i] + " " + sound_type[i] + " " + timeArr[i]);
                    // Debug.Log(count);
                    soundObjsRenderers[i].enabled = true;
                    soundObjs[i].transform.position = new Vector3(UnityEngine.Random.value *20, UnityEngine.Random.value *10,0);
                    pdPatch.SendBang(sound_type[i] + "_bang");
                }
                else{
                    soundObjsRenderers[i].enabled = false;
                }
                timeArr[i] = timeArr[i]-beat * sigArr[i];
            }
        }
    }
}
