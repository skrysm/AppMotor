// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Certificates;
using AppMotor.CoreKit.Exceptions;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Certificates.Exporting;

public sealed class TlsCertificatePublicKeyExporterTests : TlsCertificateExporterTestBase
{
    [Theory]
    [InlineData(CertificateFileFormats.PEM)]
    [InlineData(CertificateFileFormats.PFX)]
    public void Test_Export(CertificateFileFormats exportFormat)
    {
        // Setup
        using var originalCert = TlsCertificate.CreateSelfSigned("example.com", TimeSpan.FromDays(20));

        // Test our assumptions
        originalCert.HasPrivateKey.ShouldBe(true);

        // Test
        var exportedBytes = originalCert.ExportPublicKey().As(exportFormat).ToBytes();

        // Validate
        CheckExportedBytesForCorrectFormat(exportedBytes, exportFormat);

        using var reimportedCert = new TlsCertificate(TlsCertificateSource.FromBytes(exportedBytes));
        reimportedCert.PublicKey.ShouldBe(originalCert.PublicKey);
        reimportedCert.HasPrivateKey.ShouldBe(false);
    }

    [Fact]
    public void Test_ExportInvalidFormat()
    {
        // Setup
        using var originalCert = TlsCertificate.CreateSelfSigned("example.com", TimeSpan.FromDays(20));

        // Test
        Should.Throw<UnexpectedSwitchValueException>(() => originalCert.ExportPublicKey().As((CertificateFileFormats)1000));
    }
}
