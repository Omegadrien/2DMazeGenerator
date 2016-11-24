using System.Media;

namespace ProjetLabyrintheWPF
{
    class Music
    {
        private SoundPlayer player;
        private bool isPlaying = false;

        public void PlayBGM(string path)
        {
            player = new SoundPlayer(path);
            player.PlayLooping();
            isPlaying = true;
        }

        public void StopBGM()
        {
            player.Stop();
            isPlaying = false;
        }

        public bool IsPlaying()
        {
            return this.isPlaying;
        }
    }
}
