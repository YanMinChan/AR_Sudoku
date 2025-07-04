using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISoundEffectDatabase
{
    public void PlayAudio(int id, float volume = 1.0f);
}

