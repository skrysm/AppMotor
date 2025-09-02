// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Text;

using AppMotor.CoreKit.Certificates;
using AppMotor.CoreKit.Exceptions;

using Shouldly;

namespace AppMotor.CoreKit.Tests.Certificates.Exporting;

public abstract class TlsCertificateExporterTestBase
{
    private const string PEM_START = "-----BEGIN ";

    protected static void CheckExportedBytesForCorrectFormat(ReadOnlySpan<byte> exportedBytes, CertificateFileFormats exportFormat)
    {
        switch (exportFormat)
        {
            case CertificateFileFormats.PEM:
                exportedBytes[0..PEM_START.Length].ToArray().ShouldBe(Encoding.ASCII.GetBytes(PEM_START));
                break;

            case CertificateFileFormats.PFX:
                // Unfortunately, PFX doesn't seem to have any "magic numbers".
                break;

            default:
                throw new UnexpectedSwitchValueException(nameof(exportFormat), exportFormat);
        }
    }
}
