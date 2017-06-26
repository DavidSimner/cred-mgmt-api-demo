using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Jose;

namespace LightningTalk.Utils
{
    internal class Jwt
    {
        private const string typ = "typ";
        private const string jwt = "JWT";
        private const string kid = "kid";
        private const string sub = "sub";
        private const string iat = "iat";
        private const string jti = "jti";

        private static readonly JwsAlgorithm Algorithm = JwsAlgorithm.ES256;
        private static readonly string KeyId = "alpha1";
        private static readonly CngKey Key = CngKey.Import(Convert.FromBase64String(Environment.GetEnvironmentVariable("THE_PRIVATE_JWT_KEY")), CngKeyBlobFormat.EccPrivateBlob);

        internal readonly string subject;
        private readonly long issuedAt;
        private readonly Guid jwtId;

        internal static Jwt Decode(string token)
        {
            var payload = JWT.Decode<IDictionary<string, object>>(token, Key, Algorithm);
            return new Jwt((string) payload[sub], (int) payload[iat], Guid.Parse((string) payload[jti]));
        }

        internal Jwt(string subject)
            : this(subject, DateTime.UtcNow.GetTotalSecondsSinceEpoch(), Guid.NewGuid())
        {
        }

        private Jwt(string subject, long issuedAt, Guid jwtId)
        {
            this.subject = subject;
            this.issuedAt = issuedAt;
            this.jwtId = jwtId;
        }

        internal string Encode()
        {
            return JWT.Encode(Payload, Key, Algorithm, ExtraHeaders);
        }
        private IDictionary<string, object> ExtraHeaders => new Dictionary<string, object> { { typ, jwt }, { kid, KeyId } };
        private IDictionary<string, object> Payload => new Dictionary<string, object> { { sub, subject }, { iat, issuedAt }, { jti, jwtId } };
    }
}
