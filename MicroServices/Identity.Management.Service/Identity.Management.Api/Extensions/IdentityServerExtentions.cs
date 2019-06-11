using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Identity.Management.Api.Extensions
{
    public static class IdentityServerExtentions
    {

        private const string SigninKeyCredentials = "SigninKeyCredentials";

        private const string KeyType = "KeyType";

        private const string KeyTypeKeyFile = "KeyFile";
        private const string KeyFilePath = "KeyFilePath";
        private const string KeyFilePassword = "KeyFilePassword";

        private const string KeyTypeTemporary = "Temporary";

        private const string KeyTypeKeyStore = "KeyStore";
        private const string KeyStoreIssuer = "KeyStoreIssuer";

        public static IIdentityServerBuilder AddSigninCredentialFromConfig(this IIdentityServerBuilder builder,IConfiguration configuration,ILogger logger)
        {
            var keyType = configuration.GetSection(SigninKeyCredentials).GetValue<string>(KeyType);

            
            switch (keyType)
            {
                case KeyTypeTemporary:
                    {
                        builder.AddDeveloperSigningCredential();
                        break;
                    }
                case KeyTypeKeyFile:
                    {
                        AddCertificateFromFile(builder, configuration, logger);
                        break;
                    }
                case KeyTypeKeyStore:
                    {
                        AddCertificateFromStore(builder, configuration, logger);
                        break;
                    }
            }
            return builder;
        }

        private static void AddCertificateFromStore(IIdentityServerBuilder builder,
            IConfiguration configuration, ILogger logger)
        {
            var keyIssuer = configuration.GetSection(SigninKeyCredentials).GetValue<string>(KeyStoreIssuer);
            logger.LogDebug($"SigninCredentialExtension adding key from store by {keyIssuer}");

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(X509FindType.FindByIssuerName, keyIssuer, true);

            if (certificates.Count > 0)
                builder.AddSigningCredential(certificates[0]);
            else
                logger.LogError("A matching key couldn't be found in the store");
        }

        private static void AddCertificateFromFile(IIdentityServerBuilder builder,
            IConfiguration configuration, ILogger logger)
        {
            var keyFilePath = configuration.GetSection(SigninKeyCredentials).GetValue<string>(KeyFilePath);
            var keyFilePassword = configuration.GetSection(SigninKeyCredentials).GetValue<string>(KeyFilePassword);

            if (File.Exists(keyFilePath))
            {
                logger.LogDebug($"SigninCredentialExtension adding key from file {keyFilePath}");
                builder.AddSigningCredential(new X509Certificate2(keyFilePath, keyFilePassword));
            }
            else
            {
                logger.LogError($"SigninCredentialExtension cannot find key file {keyFilePath}");
            }
        }
    }
}
