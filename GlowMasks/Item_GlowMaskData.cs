namespace TLoZ.GlowMasks
{
    public class GlowMaskData
    {
        public int Width;
        public int Height;
        public int FrameCount;
        public string TexturePath;
        public int FrameSpeed;
        public string Mod;
        public GlowMaskData(string p, string m, int w, int h, int FC = 0, int FSpeed = 7)
        {
            Mod = m;
            FrameSpeed = FSpeed;
            TexturePath = p;
            Width = w;
            Height = h;
            FrameCount = FC;
        }
    }
}
