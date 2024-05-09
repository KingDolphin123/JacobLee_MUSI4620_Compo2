using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrumPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public LibPdInstance pdPatch;
    [SerializeField] public int beat;
    [SerializeField] List<bool> kickSeq = new List<bool> ( new bool[48] );
    [SerializeField] List<bool> snareSeq = new List<bool> ( new bool[48] );
    [SerializeField] List<bool> hihatSeq = new List<bool> ( new bool[48] );
    [SerializeField] List<bool> bassHighSeq = new List<bool> ( new bool[64] );
    [SerializeField] List<bool> bassLowSeq = new List<bool> ( new bool[64] );
    [SerializeField] bool kickBool;
    [SerializeField] bool snareBool;
    [SerializeField] bool hihatBool;
    [SerializeField] bool bassBool;
    string[] sound_type = new string[] { 
        "kick",
        "snare",
        "hihat",
        "basshigh",
        "basslow"
        };
    public List<AudioClip> sounds;
    float t;
    int count = 0;
    int countBass = 0;
    Material mat;
    float move;
    float density;
    float muliplyer = 0.1f;
    float hihatnoise = -8.4f;
    void Start()
    {
        move = 0;
        Transform bg = getOneChild(transform);
        mat = bg.transform.GetComponent<Renderer>().material;
        Debug.Log(mat);
        // Debug.Log("Child Object: " + getOneChild(transform));
        for (int i = 0; i < sounds.Count; i++)
        {
            string name = sounds[i].name + ".wav";
            pdPatch.SendSymbol(sound_type[i], name);
        }
        beat /= 8;
    }

    Transform getOneChild(Transform a)
    {
        foreach (Transform b in a)
        {
            return b;
        }
        return null;
        
    }
    // Update is called once per frame
    void Update()
    {
        if(kickBool){
            mat.SetFloat("_noiseOffset", move);
        }
        if(snareBool){
            mat.SetFloat("_cellDensity", density);
        }
        if(hihatBool){
            mat.SetFloat("_noiseTwo", hihatnoise);
        }
        // Debug.Log(mat.GetFloat("_noiseOffset"));
        // Debug.Log(move);
        density += 0.025f - ((density - 7) * muliplyer) ;
        move -= 0.2f + (move * muliplyer);
        hihatnoise += Random.Range(-.1f, .1f);

        float time = (int)(Time.smoothDeltaTime *1000);
        // time = (float)time/60;
        // Debug.Log(t);
        t+=time;
        if(t>beat){
            t = t-beat;
            if(kickSeq[count]){
                move = 0;
                if (kickBool){
                    pdPatch.SendBang("kick_bang");
                }
            }
            if(snareSeq[count]){
                density = 7;
                if (snareBool)
                pdPatch.SendBang("snare_bang");
            }
            if(hihatSeq[count]){
                if (hihatBool)
                pdPatch.SendBang("hihat_bang");
            }
            if(bassHighSeq[countBass]){
                if (bassBool)
                pdPatch.SendBang("basshigh_bang");
            }
            if(bassLowSeq[countBass]){
                if (bassBool)
                pdPatch.SendBang("basslow_bang");
            }
            count+=1;
            countBass+=1;
            if(count > 47){
                count = 0;
            }
            if(countBass > 63){
                countBass = 0;
            }
        }
    }
}
