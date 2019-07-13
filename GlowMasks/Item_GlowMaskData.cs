namespace TLoZ.GlowMasks
{
    public class GlowMaskData
    {
        public int width;
        public int height;
        public int frameCount;
        public string texturePath;
        public int frameSpeed;
        public string mod;
        public GlowMaskData(string p, string m, int w, int h, int fc = 0, int fSpeed = 7)
        {
            mod = m;
            frameSpeed = fSpeed;
            texturePath = p;
            width = w;
            height = h;
            frameCount = fc;
        }
    }
}
