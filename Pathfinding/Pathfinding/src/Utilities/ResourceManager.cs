using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Utilities
{
    static class ResourceManager
    {
        private static readonly SortedDictionary<string, Texture2D> textures = new SortedDictionary<string, Texture2D>();
        private static readonly SortedDictionary<string, SpriteFont> fonts = new SortedDictionary<string, SpriteFont>();
        private static readonly SortedDictionary<string, SoundEffectInstance> soundEffects = new SortedDictionary<string, SoundEffectInstance>();

        public static void AddTexture(string textureName, Texture2D texture)
        {
            textures.Add(textureName, texture);
        }
        public static void RemoveTexture(string textureName)
        {
            textures.Remove(textureName);
        }
        public static Texture2D RequestTexture(string textureName)
        {
            if (textures.ContainsKey(textureName)) //Check if list contains the texture
            {
                return textures[textureName]; //Return texture
            }
            return textures["Null"]; //ERROR
        }

        public static void AddFont(string fontName, SpriteFont font)
        {
            fonts.Add(fontName, font);
        }
        public static void RemoveFont(string fontName)
        {
            fonts.Remove(fontName);
        }
        public static SpriteFont RequestFont(string fontName)
        {
            if (fonts.ContainsKey(fontName))
            {
                return fonts[fontName];
            }
            return null;
        }

        public static void AddSound(string soundName, SoundEffect soundEffect)
        {
            soundEffects.Add(soundName, soundEffect.CreateInstance());
        }

        public static void RemoveSound(string soundName)
        {
            soundEffects.Remove(soundName);
        }
        public static SoundEffectInstance RequestSound(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                return soundEffects[soundName];
            }
            return null; //ERROR
        }

        public static void PlaySound(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                if (soundEffects[soundName].State != SoundState.Playing)
                {
                    soundEffects[soundName].Play();
                }
            }
        }
        public static void StopSound(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                if (soundEffects[soundName].State == SoundState.Playing)
                {
                    soundEffects[soundName].Stop();
                }
            }
        }
    }
}
