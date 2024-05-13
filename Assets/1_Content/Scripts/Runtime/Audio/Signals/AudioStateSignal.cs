namespace BH.Runtime.Audio
{
    public struct AudioStateSignal
    {
        public AudioState AudioState { get; }
        
        public AudioStateSignal(AudioState audioState)
        {
            AudioState = audioState;
        }
    }
}