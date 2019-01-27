using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


    public class AudioController : MonoBehaviour
    {
        
        public AudioMixerSnapshot outOfTrigger;
        public AudioMixerSnapshot inTrigger;
        // public AudioClip[] stings;
        // gpublic AudioSource stingSource;
        public float bpm = 128;

        private float m_TransitionIn;
        private float m_TransitionOut; // fade in and out times
        private float m_QuarterNote; // use to calculate lenght of quarter note in ms
        
        
        void Start()
        {
            m_QuarterNote = 60 / bpm;
            m_TransitionIn = m_QuarterNote;
            m_TransitionOut = m_QuarterNote * 32; // 8 bars fade out
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TriggerZone"))
            {
                inTrigger.TransitionTo(m_TransitionIn); // takes a time to reach
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("TriggerZone"))
            {
                outOfTrigger.TransitionTo(m_TransitionOut);
            }
        }

        
    }
