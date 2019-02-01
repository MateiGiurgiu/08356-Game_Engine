using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    public class ComponentSound : IComponent
    {
        Audio audio;
        public List<int> souce = new List<int>();

        public ComponentSound(string[] soundName)
        {
            for (int i = 0; i < soundName.Length; i++)
            {
                this.audio = ResourceManager.Loadsound(soundName[i]);
                this.souce.Add(audio.mySource);
            }
        }

        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_SOUND;

        //sound options
        public void playsoundonce(int effect)
        {
            AL.SourcePlay(souce[effect]);
        }
        public void playsoundloop(int effect)
        {
            AL.Source(souce[effect], ALSourceb.Looping, true);
            AL.SourcePlay(souce[effect]);
        }
        public void playsoundstop(int effect)
        {
            AL.SourcePause(souce[effect]);
        }

        public Audio sound()
        {
            return audio;
        }
    }
}

