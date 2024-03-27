using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Player/Audio Fader")]
    public class HFPS_AudioFader : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_AudioFader instance;


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum FadeType {

            None = 0,
            Ambience = 1,
            Music = 2,

        }//FadeType


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public float fadeMulti;

        public AudioSource ambience;
        public AudioSource music;


        public FadeType fadeType;
        public AudioSource curSource;
        public AudioClip tempClip;
        public float tempVolume;
        public bool tempKeep;

        public AudioClip oldAmbClip;
        public AudioClip oldMusicClip;

        public float oldAmbVolume;
        public float oldMusicVolume;

        public bool isFading;

        public bool ambienceIsPlaying;
        public bool musicIsPlaying;

        public bool ambienceWasPlaying;
        public bool musicWasPlaying;

        public bool ambienceDoneFading;
        public bool musicDoneFading;

        public bool revert;
        public bool locked;

        public int tabs;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            instance = this;

        }//awake

        void Start() {

            StartInit();

        }//start

        public void StartInit(){

            fadeType = FadeType.None;
            curSource = null;
            tempClip = null;
            tempVolume = 0;
            tempKeep = false;

            oldAmbClip = null;
            oldMusicClip = null;
            oldAmbVolume = 0;
            oldMusicVolume = 0;

            isFading = false;

            musicIsPlaying = false;
            ambienceIsPlaying = false;

            musicWasPlaying = false;
            ambienceWasPlaying = false;

            ambienceDoneFading = false;
            musicDoneFading = false;

            revert = false;
            locked = false;

        }//StartInit


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update() {

            if(!locked){

                if(isFading){

                    if(fadeType == FadeType.Ambience){

                        if(!revert){

                            if(ambienceWasPlaying && !ambienceDoneFading){

                                if(ambience.volume > 0.01f){

                                    ambience.volume -= fadeMulti * Time.deltaTime;

                                }//volume > 0.01f

                                if(ambience.volume <= 0.01f){

                                    ambience.volume = 0;
                                    ambience.Stop();

                                    ambienceDoneFading = true;

                                }//volume <= 0.01f

                            }//ambienceWasPlaying & !ambienceDoneFading

                            if(musicWasPlaying){

                                if(!musicDoneFading){

                                    if(music.volume > 0.01f){

                                        music.volume -= fadeMulti * Time.deltaTime;

                                    }//volume > 0.01f

                                    if(music.volume <= 0.01f){

                                        music.volume = 0;
                                        music.Stop();

                                        musicDoneFading = true;

                                    }//volume <= 0.01f

                                }//!!musicDoneFading

                            //musicWasPlaying
                            } else {

                                musicDoneFading = true;

                            }//musicWasPlaying

                            if(ambienceDoneFading && musicDoneFading){

                                if(ambience.clip != tempClip){

                                    ambience.clip = tempClip;

                                }//clip != tempClip

                                if(ambience.clip = tempClip){

                                    if(!ambience.isPlaying){

                                        ambience.Play();

                                        ambienceIsPlaying = true;

                                    }//!isPlaying

                                    if(ambience.volume < tempVolume){

                                        ambience.volume += fadeMulti * Time.deltaTime;

                                    }//volume < tempVolume

                                    if(ambience.volume >= tempVolume){

                                        ambience.volume = tempVolume;

                                        isFading = false;

                                    }//volume <= 0.01f

                                }//clip != tempClip

                            }//ambienceDoneFading & musicDoneFading

                        //!revert
                        } else {

                            if(ambienceIsPlaying && !ambienceDoneFading){

                                if(ambience.volume > 0.01f){

                                    ambience.volume -= fadeMulti * Time.deltaTime;

                                }//volume > 0.01f

                                if(ambience.volume <= 0.01f){

                                    ambience.volume = 0;
                                    ambience.Stop();

                                    ambienceDoneFading = true;

                                }//volume <= 0.01f

                            }//ambienceIsPlaying & !ambienceDoneFading

                            if(ambienceWasPlaying && ambienceDoneFading){

                                if(ambience.clip != oldAmbClip){

                                    ambience.clip = oldAmbClip;

                                }//clip != oldAmbClip

                                if(ambience.clip = oldAmbClip){

                                    if(!ambience.isPlaying){

                                        ambience.Play();

                                    }//!isPlaying

                                    if(ambience.volume < oldAmbVolume){

                                        ambience.volume += fadeMulti * Time.deltaTime;

                                    }//volume < oldAmbVolume

                                    if(ambience.volume >= oldAmbVolume){

                                        ambience.volume = oldAmbVolume;

                                        ambienceIsPlaying = false;

                                    }//volume >= oldAmbVolume

                                }//clip != oldAmbClip

                            }//ambienceWasPlaying & ambienceDoneFading

                            if(musicWasPlaying && ambienceDoneFading){

                                if(music.clip != oldMusicClip){

                                    music.clip = oldMusicClip;

                                }//clip != oldMusicClip

                                if(music.clip = oldMusicClip){

                                    if(!music.isPlaying){

                                        music.Play();

                                    }//!isPlaying

                                    if(music.volume < oldMusicVolume){

                                        music.volume += fadeMulti * Time.deltaTime;

                                    }//volume < oldMusicVolume

                                    if(music.volume >= oldMusicVolume){

                                        music.volume = oldMusicVolume;

                                        musicIsPlaying = false;

                                    }//volume >= oldMusicVolume

                                }//clip != oldMusicVolume

                            }//ambienceWasPlaying & ambienceDoneFading

                            if(musicWasPlaying){

                                if(!ambienceIsPlaying && !musicIsPlaying){

                                    isFading = false;

                                }//!ambienceIsPlaying & !musicIsPlaying

                            //musicWasPlaying
                            } else {

                                if(!ambienceIsPlaying){

                                    isFading = false;

                                }//!ambienceIsPlaying

                            }//musicWasPlaying

                        }//!revert

                    }//fadeType = ambience

                    if(fadeType == FadeType.Music){

                        if(!revert){

                            if(!tempKeep){

                                if(ambienceWasPlaying && !ambienceDoneFading){

                                    if(ambience.volume > 0.01f){

                                        ambience.volume -= fadeMulti * Time.deltaTime;

                                    }//volume > 0.01f

                                    if(ambience.volume <= 0.01f){

                                        ambience.volume = 0;
                                        ambience.Stop();

                                        ambienceDoneFading = true;

                                    }//volume <= 0.01f

                                }//ambienceWasPlaying

                                if(musicWasPlaying){

                                    if(!musicDoneFading){

                                        if(music.volume > 0.01f){

                                            music.volume -= fadeMulti * Time.deltaTime;

                                        }//volume > 0.01f

                                        if(music.volume <= 0.01f){

                                            music.volume = 0;
                                            music.Stop();

                                            musicDoneFading = true;

                                        }//volume <= 0.01f

                                    }//!musicDoneFading

                                //musicWasPlaying
                                } else {

                                    musicDoneFading = true;

                                }//musicWasPlaying

                                if(ambienceDoneFading && musicDoneFading){

                                    if(music.clip != tempClip){

                                        music.clip = tempClip;

                                    }//clip != tempClip

                                    if(music.clip = tempClip){

                                        if(!music.isPlaying){

                                            music.Play();

                                            musicIsPlaying = true;

                                        }//!isPlaying

                                        if(music.volume < tempVolume){

                                            music.volume += fadeMulti * Time.deltaTime;

                                        }//volume < tempVolume

                                        if(music.volume >= tempVolume){

                                            music.volume = tempVolume;

                                            isFading = false;

                                        }//volume <= 0.01f

                                    }//clip != tempClip

                                }//ambienceDoneFading & musicDoneFading

                            //!tempKeep
                            } else {

                                if(musicWasPlaying){

                                    if(!musicDoneFading){

                                        if(music.volume > 0.01f){

                                            music.volume -= fadeMulti * Time.deltaTime;

                                        }//volume > 0.01f

                                        if(music.volume <= 0.01f){

                                            music.volume = 0;
                                            music.Stop();

                                            musicDoneFading = true;

                                        }//volume <= 0.01f

                                    }//!musicDoneFading

                                //musicWasPlaying
                                } else {

                                    musicDoneFading = true;

                                }//musicWasPlaying

                                if(musicDoneFading){

                                    if(music.clip != tempClip){

                                        music.clip = tempClip;

                                    }//clip != tempClip

                                    if(music.clip = tempClip){

                                        if(!music.isPlaying){

                                            music.Play();

                                            musicIsPlaying = true;

                                        }//!isPlaying

                                        if(music.volume < tempVolume){

                                            music.volume += fadeMulti * Time.deltaTime;

                                        }//volume < tempVolume

                                        if(music.volume >= tempVolume){

                                            music.volume = tempVolume;

                                            isFading = false;

                                        }//volume <= 0.01f

                                    }//clip != tempClip

                                }//musicDoneFading

                            }//!tempKeep

                        //!revert
                        } else {

                            if(!tempKeep){

                                if(ambienceIsPlaying){

                                    if(!ambienceDoneFading){

                                        if(ambience.volume > 0.01f){

                                            ambience.volume -= fadeMulti * Time.deltaTime;

                                        }//volume > 0.01f

                                        if(ambience.volume <= 0.01f){

                                            ambience.volume = 0;
                                            ambience.Stop();

                                            ambienceDoneFading = true;

                                        }//volume <= 0.01f

                                    }//!ambienceDoneFading

                                //ambienceIsPlaying
                                } else {

                                    ambienceDoneFading = true;

                                }//ambienceIsPlaying

                                if(musicIsPlaying && !musicDoneFading){

                                    if(music.volume > 0.01f){

                                        music.volume -= fadeMulti * Time.deltaTime;

                                    }//volume > 0.01f

                                    if(music.volume <= 0.01f){

                                        music.volume = 0;
                                        music.Stop();

                                        musicDoneFading = true;

                                    }//volume <= 0.01f

                                }//musicWasPlaying & !musicDoneFading

                                if(ambienceWasPlaying && ambienceDoneFading){

                                    if(ambience.clip != oldAmbClip){

                                        ambience.clip = oldAmbClip;

                                    }//clip != oldAmbClip

                                    if(ambience.clip = oldAmbClip){

                                        if(!ambience.isPlaying){

                                            ambience.Play();

                                        }//!isPlaying

                                        if(ambience.volume < oldAmbVolume){

                                            ambience.volume += fadeMulti * Time.deltaTime;

                                        }//volume < oldAmbVolume

                                        if(ambience.volume >= oldAmbVolume){

                                            ambience.volume = oldAmbVolume;

                                            ambienceIsPlaying = false;

                                        }//volume >= oldAmbVolume

                                    }//clip != oldAmbClip

                                }//ambienceWasPlaying & ambienceDoneFading

                                if(musicWasPlaying && musicDoneFading){

                                    if(music.clip != oldMusicClip){

                                        music.clip = oldMusicClip;

                                    }//clip != oldMusicClip

                                    if(music.clip = oldMusicClip){

                                        if(!music.isPlaying){

                                            music.Play();

                                        }//!isPlaying

                                        if(music.volume < oldMusicVolume){

                                            music.volume += fadeMulti * Time.deltaTime;

                                        }//volume < oldMusicVolume

                                        if(music.volume >= oldMusicVolume){

                                            music.volume = oldMusicVolume;

                                            musicIsPlaying = false;

                                        }//volume >= oldMusicVolume

                                    }//clip != oldMusicVolume

                                }//musicWasPlaying & musicDoneFading

                                if(!ambienceIsPlaying && !musicIsPlaying){

                                    isFading = false;

                                }//!ambienceIsPlaying & !musicIsPlaying

                            //!tempKeep
                            } else {

                                if(musicIsPlaying && !musicDoneFading){

                                    if(music.volume > 0.01f){

                                        music.volume -= fadeMulti * Time.deltaTime;

                                    }//volume > 0.01f

                                    if(music.volume <= 0.01f){

                                        music.volume = 0;
                                        music.Stop();

                                        musicDoneFading = true;

                                    }//volume <= 0.01f

                                }//musicWasPlaying & !musicDoneFading

                                if(musicWasPlaying && musicDoneFading){

                                    if(music.clip != oldMusicClip){

                                        music.clip = oldMusicClip;

                                    }//clip != oldMusicClip

                                    if(music.clip = oldMusicClip){

                                        if(!music.isPlaying){

                                            music.Play();

                                        }//!isPlaying

                                        if(music.volume < oldMusicVolume){

                                            music.volume += fadeMulti * Time.deltaTime;

                                        }//volume < oldMusicVolume

                                        if(music.volume >= oldMusicVolume){

                                            music.volume = oldMusicVolume;

                                            musicIsPlaying = false;

                                        }//volume >= oldMusicVolume

                                    }//clip != oldMusicVolume

                                }//musicWasPlaying & musicDoneFading

                                if(!musicIsPlaying){

                                    isFading = false;

                                }//!musicIsPlaying

                            }//!tempKeep

                        }//!revert

                    }//fadeType = music

                }//isFading

            }//locked

        }//update


    //////////////////////////////////////
    ///
    ///     FADE ACTIONS
    ///
    ///////////////////////////////////////


        public void Fade_Start(FadeType newType, AudioClip newClip, float volume, bool keepAmbience, bool immediate){

            ambienceIsPlaying = false;
            musicIsPlaying = false;

            ambienceWasPlaying = false;
            musicWasPlaying = false;

            ambienceDoneFading = false;
            musicDoneFading = false;

            revert = false;

            fadeType = newType;

            tempClip = newClip;

            if(volume != 0){

                tempVolume = volume;

            }//volume != 0

            tempKeep = keepAmbience;

            if(!immediate){

                if(ambience.isPlaying){

                    oldAmbClip = ambience.clip;
                    oldAmbVolume = ambience.volume;

                    if(volume == 0){

                        tempVolume = oldAmbVolume;

                    }//volume = 0

                    ambienceWasPlaying = true;

                }//isPlaying

                if(music.isPlaying){

                    oldMusicClip = music.clip;
                    oldMusicVolume = music.volume;

                    if(volume == 0){

                        tempVolume = oldMusicVolume;

                    }//volume = 0

                    musicWasPlaying = true;

                }//isPlaying

                isFading = true;

            //!immediate
            } else {

                if(fadeType == FadeType.Ambience){

                    ambience.clip = newClip;

                    if(volume > 0){

                        ambience.volume = volume;

                    }//volume > 0

                    ambience.Play();

                }//fadeType = ambience

                if(fadeType == FadeType.Music){

                    music.clip = newClip;

                    if(volume > 0){

                        music.volume = volume;

                    }//volume > 0

                    music.Play();

                }//fadeType = music

            }//!immediate

        }//Fade_Start

        public void Fade_Revert(){

            ambienceDoneFading = false;
            musicDoneFading = false;

            revert = true;
            isFading = true;

        }//Fade_Revert


    }//HFPS_AudioFader


}//namespace