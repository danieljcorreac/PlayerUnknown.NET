namespace PlayerUnknown.Offsets
{
    public class UMaterialShaderQualitySettings
    {
        ///<summary>TMap&lt;FName,UShaderPlatformQualitySettings * &gt;</summary>
        public const int ForwardSettingMap = 0x0028;
    }

    public class UShaderPlatformQualitySettings
    {
        ///<summary>FMaterialQualityOverrides[0x3]</summary>
        public const int QualityOverrides = 0x0028;

        ///<summary>unsigned char[0x6]</summary>
        public const int UnknownData00 = 0x003A;
    }
}