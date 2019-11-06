using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWarsFighter
{
    public class GameAudio
    {
        //Audio Engine Items
        private AudioEngine m_Engine;
        //private WaveBank m_WaveBankE;
        //private WaveBank m_WaveBankR;
        private WaveBank m_WaveBankP;
        private SoundBank m_SoundBank;
        //private SoundBank m_SoundBankE;
        //private SoundBank m_SoundBankR;
        private SoundBank m_SoundBankP;
        //private SoundBank m_oldBank;

        public List<Cue> audio_MainMenu;
        public List<Cue> audio_PauseMenu;
        public List<Cue> audio_inGame;
        //New Audio Struct
        public struct audioItem
        {
            public Cue audioCue;
            public bool isPlaying;
            public bool isPaused;
            //public string soundBank;
        };

        //BLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLSSSSSSSSSSSSSSSSSS
        //List of game audio
        public List<audioItem> m_gameAudio = new List<audioItem>();
        /// <summary>
        /// GameAudio constructor
        /// </summary>
        public GameAudio(string soundBank)
        {
            m_Engine = new AudioEngine("Content\\audio.xgs");
            //m_WaveBankE = new WaveBank(m_Engine, "Content\\Empire.xwb");
            //m_WaveBankR = new WaveBank(m_Engine, "Content\\Rebels.xwb");
            m_WaveBankP = new WaveBank(m_Engine, "Content\\Ships.xwb");
            m_SoundBank = new SoundBank(m_Engine, "Content\\"+soundBank+".xsb");
            //m_SoundBankE = new SoundBank(m_Engine, "Content\\Empire.xsb");
            //m_SoundBankR = new SoundBank(m_Engine, "Content\\Rebels.xsb");
            m_SoundBankP = new SoundBank(m_Engine, "Content\\"+ soundBank+".xsb");

            audio_inGame = new List<Cue>();
            audio_MainMenu = new List<Cue>();
            audio_PauseMenu = new List<Cue>();
        }

        public bool isSoundPlaying(string sound)
        {
            bool found = false;
            int position = 0;
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].audioCue.Name == sound)
                {
                    //Found sprite!
                    found = true;
                    position = i;
                }
            }
            if (found)
            {
               // if (StarWarsFighter.debug) ;//Console.WriteLine("Sound:" + sound + " is " + m_gameAudio[position].isPlaying);
                return m_gameAudio[position].isPlaying;
            }
            else
            {
               // if (StarWarsFighter.debug) ; //Console.WriteLine("isSoundPlaying(" + sound + ")" + "couldnt be found");
                return false;
            }
        }
        /// <summary>
        /// GameAudio destructor
        /// </summary>
        ~GameAudio()
        {
            m_SoundBank.Dispose();
            //m_SoundBankE.Dispose();
            //m_SoundBankR.Dispose();
            m_SoundBankP.Dispose();
            //m_oldBank.Dispose();
            m_WaveBankP.Dispose();
            m_Engine.Dispose();
        }
        /*public void switchSoundBanks(string soundB)
        {
            m_oldBank = m_SoundBank;
            switch (soundB)
            {
                case "Empire":
                    m_SoundBank = m_SoundBankE;
                    break;
                case "Rebels":
                    m_SoundBank = m_SoundBankR;
                    break;
                case "Player":
                    m_SoundBank = m_SoundBankP;
                    break;
            }
        }*/
        /// <summary>
        /// Adds a sound using sound filename
        /// </summary>
        /// <param name="sound"></param>
        public void AddSound(String sound)
        {
            if (isSoundAdded(sound))
                return;
            audioItem newAudio;
            //switchSoundBanks(soundB);
            newAudio.audioCue = m_SoundBank.GetCue(sound);

            newAudio.isPaused = false;
            newAudio.isPlaying = false;
            //newAudio.soundBank = soundB;
            m_gameAudio.Add(newAudio);
            //m_SoundBank = m_oldBank;
        }
        public bool findSound(string sound)
        {
            try
            {
                Cue s = m_SoundBank.GetCue(sound);
                //Console.WriteLine(s.Name + " found");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(sound + " not found");
                return false;
            }
        }
        /// <summary>
        /// Adds a sound using sound filename
        /// </summary>
        /// <param name="sound"></param>
        public void RemoveSound(String sound)
        {
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].audioCue.Name == sound)
                {
                    //Console.WriteLine("Found " + sound);
                    PauseSound(sound);
                    StopSound(sound);
                    m_gameAudio.Remove(m_gameAudio[i]);
                    // m_gameAudio[i] = null;
                    break;
                }
            }

        }
        public void RemoveAll()
        {
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                RemoveSound(m_gameAudio[i].audioCue.Name);
            }

        }
        /// <summary>
        /// Plays a sound based on sound filename
        /// </summary>
        /// <param name="sound"></param>
        public void PlaySound(String sound)
        {
            //Default variables
            int position = 0;
            bool found = false;
            //Look for sound in list
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].audioCue.Name == sound)
                {
                    //Found sprite!
                    found = true;
                    position = i;
                }
            }
            //Can't update without an old sound object
            if (found)
            {
                audioItem localvar;
                localvar.isPaused = false;
                localvar.isPlaying = true;
                //localvar.soundBank = m_gameAudio[position].soundBank;

               // switchSoundBanks(localvar.soundBank);
                /*m_oldBank = m_SoundBank;
                m_SoundBank = new SoundBank(m_Engine,"Content\\" ++".xsb");*/
                localvar.audioCue =
                m_SoundBank.GetCue(m_gameAudio[position].audioCue.Name);

                m_gameAudio[position] = localvar;
                m_gameAudio[position].audioCue.Play();
               // m_SoundBank = m_oldBank;
            }
            else
            {
                Console.WriteLine("Couldnt find " + sound);
            }
        }
        /// <summary>
        /// Pauses specified sound
        /// </summary>
        /// <param name="sound"></param>
        public void PauseSound(String sound)
        {
            //Default variables
            int position = 0;
            bool found = false;
            //Look for sound in list
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].audioCue.Name == sound)
                {
                    //Found sprite!
                    found = true;
                    position = i;
                }
            }
            //Can't update without an old sound object
            if (found && m_gameAudio[position].isPlaying)
            {
                audioItem localvar;
                localvar.isPaused = true;
                localvar.isPlaying = false;
               // localvar.soundBank = m_gameAudio[position].soundBank;
                localvar.audioCue = m_gameAudio[position].audioCue;
                m_gameAudio[position] = localvar;
                m_gameAudio[position].audioCue.Pause();
            }
        }
        /// <summary>
        /// Resumes specified sound
        /// </summary>
        /// <param name="sound"></param>
        public void UnpauseSound(String sound)
        {
            //Default variables
            int position = 0;
            bool found = false;
            //Look for sound in list
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].audioCue.Name == sound)
                {
                    //Found sprite!
                    found = true;
                    position = i;
                }
            }
            //Can't update without an old sound object
            if (found && m_gameAudio[position].isPaused)
            {
                audioItem localvar;
                localvar.isPaused = false;
                localvar.isPlaying = true;
                //localvar.soundBank = m_gameAudio[position].soundBank;
                localvar.audioCue = m_gameAudio[position].audioCue;
                m_gameAudio[position] = localvar;
                m_gameAudio[position].audioCue.Resume();
            }
        }
        /// <summary>
        /// Stops specified sound
        /// </summary>
        /// <param name="sound"></param>
        public void StopSound(String sound)
        {
            //Default variables
            int position = 0;
            bool found = false;
            //Look for sound in list
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].audioCue.Name == sound)
                {
                    //Found sprite!
                    found = true;
                    position = i;
                    break;
                }
            }
            //Can't update without an old sound object
            if (found && m_gameAudio[position].isPlaying)
            {
                audioItem localvar;
                localvar.isPaused = false;
                localvar.isPlaying = false;
                //localvar.soundBank = m_gameAudio[position].soundBank;
                localvar.audioCue = m_gameAudio[position].audioCue;
                m_gameAudio[position] = localvar;
                m_gameAudio[position].audioCue.Stop(AudioStopOptions.Immediate);
            }

            else if (!found)
            {

            }
        }
        /// <summary>
        /// Stops all playing/paused sounds
        /// </summary>
        public void StopAll()
        {
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].isPlaying || m_gameAudio[i].isPaused)
                {
                    audioItem localvar;
                    localvar.isPaused = false;
                    localvar.isPlaying = false;
                   // localvar.soundBank = m_gameAudio[i].soundBank;
                    localvar.audioCue = m_gameAudio[i].audioCue;
                    m_gameAudio[i] = localvar;
                    m_gameAudio[i].audioCue.Stop(AudioStopOptions.Immediate);
                }
            }
        }
        /// <summary>
        /// Pauses all playing sounds
        /// </summary>
        public void PauseAll()
        {
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].isPlaying)
                {
                    audioItem localvar;
                    localvar.isPaused = true;
                    localvar.isPlaying = false;
                    //localvar.soundBank = m_gameAudio[i].soundBank;
                    localvar.audioCue = m_gameAudio[i].audioCue;
                    m_gameAudio[i] = localvar;
                    try
                    {
                        m_gameAudio[i].audioCue.Pause();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("---------------------------");
                        Console.WriteLine("PauseAll() Error:");
                        Console.WriteLine(e.Data);
                        Console.WriteLine("---------------------------");
                    }
                }
            }
        }
        /// <summary>
        /// Unpauses all paused sounds
        /// </summary>
        public void UnpauseAll()
        {
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].isPaused)
                {
                    audioItem localvar;
                    localvar.isPaused = false;
                    localvar.isPlaying = true;
                    //localvar.soundBank = m_gameAudio[i].soundBank;
                    localvar.audioCue = m_gameAudio[i].audioCue;
                    m_gameAudio[i] = localvar;
                    m_gameAudio[i].audioCue.Resume();
                }
            }
        }
        /// <summary>
        /// Updates Sound Engine
        /// </summary>
        public void UpdateAudio()
        {
            m_Engine.Update();
        }
        public bool isSoundAdded(string sound)
        {
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].audioCue.Name==sound)
                {
                    return true;
                }
            }
            return false;
        }
        public void setAudioList(StarWarsFighter.State state)
        {
            switch (state)
            {
                case StarWarsFighter.State.inGame:
                    audio_inGame = new List<Cue>();
                    for (int i = 0; i < m_gameAudio.Count; i++)
                    {
                        if (m_gameAudio[i].audioCue.IsPlaying)
                        {
                            //StopSound(m_gameAudio[i].audioCue.Name);
                            audio_inGame.Add(m_gameAudio[i].audioCue);
                        }
                    }
                    break;
                case StarWarsFighter.State.inMainMenu:
                    audio_MainMenu = new List<Cue>();
                    for (int i = 0; i < m_gameAudio.Count; i++)
                    {
                        if (m_gameAudio[i].audioCue.IsPlaying)
                        {
                            //StopSound(m_gameAudio[i].audioCue.Name);
                            audio_MainMenu.Add(m_gameAudio[i].audioCue);
                        }
                    }
                    break;
                case StarWarsFighter.State.inPauseMenu:
                    audio_PauseMenu = new List<Cue>();
                    for (int i = 0; i < m_gameAudio.Count; i++)
                    {
                        if (m_gameAudio[i].audioCue.IsPlaying)
                        {
                            //StopSound(m_gameAudio[i].audioCue.Name);
                            audio_PauseMenu.Add(m_gameAudio[i].audioCue);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        public void printAllPlaying()
        {
            Console.WriteLine();
            Console.Write("playing:");
            for (int i = 0; i < m_gameAudio.Count; i++)
            {
                if (m_gameAudio[i].isPlaying)
                {
                    Console.Write(m_gameAudio[i].audioCue.Name+",");
                }
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
        }
        public void printAudiolist(StarWarsFighter.State state)
        {

            Console.WriteLine();
            switch (state)
            {
                case StarWarsFighter.State.inGame:
                    Console.Write("inGame:");
                    for (int i = 0; i < audio_inGame.Count; i++)
                    {
                        
                            Console.Write(audio_inGame[i].Name + ",");
                        
                    }
                    break;
                case StarWarsFighter.State.inMainMenu:
                    Console.Write("MainMenu:");
                    for (int i = 0; i < audio_MainMenu.Count; i++)
                    {
                        
                            Console.Write(audio_MainMenu[i].Name + ",");
                        
                    }
                    break;
                case StarWarsFighter.State.inPauseMenu:
                    Console.Write("PauseMenu:");
                    for (int i = 0; i < audio_PauseMenu.Count; i++)
                    {
                        
                            Console.Write(audio_PauseMenu[i].Name + ",");
                        
                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
        }
        public void playAudioList(StarWarsFighter.State newState)
        {
            StopAll();
            switch (newState)
            {
                case StarWarsFighter.State.inGame:
                    audio_inGame.ForEach(a => 
                    {
                        PlaySound(a.Name);
                    });
                    break;
                case StarWarsFighter.State.inMainMenu:
                    audio_MainMenu.ForEach(a =>
                    {
                        PlaySound(a.Name);
                    });
                    break;
                case StarWarsFighter.State.inPauseMenu:
                    audio_PauseMenu.ForEach(a =>
                    {
                        PlaySound(a.Name);
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
