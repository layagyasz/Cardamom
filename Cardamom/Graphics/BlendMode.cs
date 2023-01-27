using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    public struct BlendMode
    {
        public static readonly BlendMode Add =
            new(
                BlendEquationMode.FuncAdd,
                BlendingFactorSrc.SrcAlpha,
                BlendingFactorDest.One,
                BlendingFactorSrc.One,
                BlendingFactorDest.One);

        public static readonly BlendMode Alpha = 
            new(
                BlendEquationMode.FuncAdd,
                BlendingFactorSrc.SrcAlpha,
                BlendingFactorDest.OneMinusSrcAlpha,
                BlendingFactorSrc.One, 
                BlendingFactorDest.OneMinusSrcAlpha);

        public static readonly BlendMode Max =
            new(
                BlendEquationMode.Max,
                BlendingFactorSrc.One,
                BlendingFactorDest.One,
                BlendingFactorSrc.One,
                BlendingFactorDest.One);

        public static readonly BlendMode Min =
            new(
                BlendEquationMode.Min,
                BlendingFactorSrc.One,
                BlendingFactorDest.One,
                BlendingFactorSrc.One,
                BlendingFactorDest.One);

        public static readonly BlendMode Multiply =
            new(
                BlendEquationMode.FuncAdd,
                BlendingFactorSrc.DstColor,
                BlendingFactorDest.Zero,
                BlendingFactorSrc.DstAlpha,
                BlendingFactorDest.Zero);

        public static readonly BlendMode None =
            new(
                BlendEquationMode.FuncAdd,
                BlendingFactorSrc.One,
                BlendingFactorDest.Zero,
                BlendingFactorSrc.One,
                BlendingFactorDest.Zero);

        public BlendEquationMode Equation { get; set; }
        public BlendingFactorSrc ColorSourceFactor { get; set; }
        public BlendingFactorDest ColorDestinationFactor { get; set; }
        public BlendingFactorSrc AlphaSourceFactor { get; set; }
        public BlendingFactorDest AlphaDestinationFactor { get; set; }

        public BlendMode(
            BlendEquationMode equation, 
            BlendingFactorSrc colorSourceFactor, 
            BlendingFactorDest colorDestinationFactor,
            BlendingFactorSrc alphaSourceFactor, 
            BlendingFactorDest alphaDestinationFactor)
        {
            Equation = equation;
            ColorSourceFactor = colorSourceFactor;
            ColorDestinationFactor = colorDestinationFactor;
            AlphaSourceFactor = alphaSourceFactor;
            AlphaDestinationFactor = alphaDestinationFactor;
        }
    }
}
