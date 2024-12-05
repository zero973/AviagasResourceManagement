using Microsoft.IdentityModel.JsonWebTokens;

namespace ARM.Core.Settings;

public static class EncryptionConstants
{
    public const string ValidEncryptionAlgorithm = JwtConstants.DirectKeyUseAlg;
    public const string ValidAlgorithm = "HS256";
}