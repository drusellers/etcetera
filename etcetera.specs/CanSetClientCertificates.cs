namespace etcetera.specs
{
    using Should;
    using Xunit;
    using System.Security.Cryptography.X509Certificates;

    public class CanGetSetClientCertificates :
        EtcdBase
    {

        readonly X509Certificate _x509Certificate;
        readonly X509CertificateCollection _x509CertificateCollection;

        public CanGetSetClientCertificates()
        {
            _x509Certificate = new X509Certificate();
            _x509CertificateCollection = new X509CertificateCollection(){ _x509Certificate };

            Client.ClientCertificates.ShouldBeNull();
            Client.ClientCertificates = _x509CertificateCollection;
        }

        [Fact]
        public void ClientCertificatesIsSet()
        {
            Client.ClientCertificates.ShouldEqual(_x509CertificateCollection);
        }

        [Fact]
        public void ClientCertificatesCanGet()
        {
            Client.ClientCertificates.Contains(_x509Certificate);
        }
    }
}
