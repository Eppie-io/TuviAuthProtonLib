////////////////////////////////////////////////////////////////////////////////
//
//   Copyright 2023 Eppie(https://eppie.io)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
////////////////////////////////////////////////////////////////////////////////

namespace Tuvi.Auth.Proton.Test.TestData
{
    public static class VerificationData
    {
        public const string RightSignedModulus = "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHA256\n\nS/hBgmVXHlpzUxgzOlt4veE3v3BnpaVyRFUUDMmRgcF2yZU5rQcQYHDBGrnQAlGdcsGmZVcZC51JgJtEB6v5bBpxnnsjg8XibZm0GYXODhm7qki5wM5AEKoTKbZKaKuRD297pPTsVdqUdXFNdkDxk3Q3nv3N6ZEJccCS1IabllN+/adVTjUfCMA9pyJavOOj90fhcCQ2npInsxegvlGvREr1JpobdrtbXAOzLH+9ELxpW91ZFWbN0HHaE8+JV8TsZnhY+W0pqL+x18iVBwOCKjqiNVlXsJsd4PV0fyX3Fb/uRTnUuEYe/98xo+qqG/CrhIW7QgiuwemEN7PdHHARnQ==\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nwl4EARYIABAFAlwB1j0JEDUFhcTpUY8mAADqcAEA3ZzmaFqnbwCxfGqupfOL\nv8s+Z2PoHQ5KjSkXzMW1RZEA/R1s3YA4h/mChLxEFYEgNHHaRqLh3fmLXY8q\nkd76+7UO\n=hVVV\n-----END PGP SIGNATURE-----\n";
        public const string RightSignedModulus2 = "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHA256\n\ncxppFHJLKpd2WRExtw197XHmR+xd67d45DWyUx8iDAbnyPZolH2jJekfI9n19GVdRPQvwBECs4mLpp6AikuwuPJ/uwzWl3Rqh6ei6QmzC1iBLAQURso7Wxy8AyE+VXLbup119yyj1JaF4KxLXU6E9mzqevPdMqYD1XI19BGHAlZGCCfjTalTyQA4eh0pIcjQmeJ/8OjfUqx/enRz4ZvuQ8KDn9+iK8b/+OqH3pQr5s6YDlatQAIStdBPCZd/XbHNaFWrAZix21J/t99mPwKpGfVya1wA6j6LS2XQ/FQYuwQLNm/ZLlMYyNEcbL0CeebRvb8Cpv09u3fXpvs2YC3xjQ==\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nwl4EARYIABAFAlwB1j0JEDUFhcTpUY8mAABHiwEAqfW3kWi+IEHDJY+GOMZd\nSYd8wE5Lgwutlsve3YaNnB8A/1eBa+93z7/teaAIqzbFcW5Ix8ce5VovZ/Qo\nvFs56ioF\n=GOG6\n-----END PGP SIGNATURE-----\n";
        public const string RightSignedModulus3 = "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHA256\n\ne6JT9qI2YWlCVAiP7dTEWzbRqynayWp47TZlh/gJ4dU9rRW81+vK0pxdJAdyUERLglfFUrevrYM9mcOXT1emr1wVuV7/S07G5G3hD/1WK5PyhiK2Wh+kct4TS5zX8aMb3hiaO60guFFIO8gCltqf9yGKiycQqDv90KryknnqdZrYL0zn35dj1oeLMjajuTpap7Jow8NWDxuXH+sARHpdUG/fPdTZ5dQEPaOBuTvz7krg6aFk+LYaOI80pW1280/N0205Asz+UYUW1TkyvJGy2MI/382pW4UFfOmahzCkxYDuEhkD8hu/84rR2IWoe5QVFFZqrWgEQImIbun/jlaB9g==\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nwl4EARYIABAFAlwB1j4JEDUFhcTpUY8mAACFmgD/ToJJvPKs8foPNKroTg1I\ntyuWpjSvuKZRdD+G4l4oozwBAJgM2xBcI8Td6Qdbgae0sZxZDrYRgtR8I/Tt\nCnxISuMJ\n=3SgM\n-----END PGP SIGNATURE-----\n";

        public const string WrongSignedModulus = "-----BEGIN PGP SIGNED MESSAGE-----\n" +
    "Hash: SHA256\n" +
    "\n" +
    "\n" +
    " W2z5HBi8RvsfYzZTS7qBaUxxPhsfHJFZpu3Kd6s1JafNrCCH9rfvPLrfuqocxWPgWDH2R8neK7PkNvjxto9TStuY5z7jAzWRvFWN9cQhAKkdWgy0JY6ywVn22+HFpF4cYesHrqFIKUPDMSSIlWjBVmEJZ/MusD44ZT29xcPrOqeZvwtCffKtGAIjLYPZIEbZKnDM1Dm3q2K/xS5h+xdhjnndhsrkwm9U9oyA2wxzSXFL+pdfj2fOdRwuR5nW0J2NFrq3kJjkRmpO/Genq1UW+TEknIWAb6VzJJJA244K/H8cnSx2+nSNZO3bbo6Ys228ruV9A8m6DhxmS+bihN3ttQ==\n" +
    "\n" +
    "-----BEGIN PGP SIGNATURE-----\n" +
    "Version: ProtonMail\n" +
    "Comment: https://protonmail.com\n" +
    "\n" +
    "wl4EARYIABAFAlwB1j0JEDUFhcTpUY8mAAD8CgEAnsFnF4cF0uSHKkXa1GIa" +
    "GO86yMV4zDZEZcDSJo0fgr8A/AlupGN9EdHlsrZLmTA1vhIx+rOgxdEff28N" +
    "kvNM7qIK\n" +
    "=q6vu\n" +
    "-----END PGP SIGNATURE-----\n";

        public const string BadFormattedModulus1 = "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHA256\n\nS/hBgmVXHlpzUxgzOlt4veE3v3BnpaVyRFUUDMmRgcF2yZU5rQcQYHDBGrnQAlGdcsGmZVcZC51JgJtEB6v5bBpxnnsjg8XibZm0GYXODhm7qki5wM5AEKoTKbZKaKuRD297pPTsVdqUdXFNdkDxk3Q3nv3N6ZEJccCS1IabllN+/adVTjUfCMA9pyJavOOj90fhcCQ2npInsxegvlGvREr1JpobdrtbXAOzLH+9ELxpW91ZFWbN0HHaE8+JV8TsZnhY+W0pqL+x18iVBwOCKjqiNVlXsJsd4PV0fyX3Fb/uRTnUuEYe/98xo+qqG/CrhIW7QgiuwemEN7PdHHARnQ==\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nwl4EARYIABAFAlwB1j0JEDUFhcTpUY8mAADqcAEA3ZzmaFqnbwCxfGqupfOL\nkd76+7UO\n=hV\n-----END PGP SIGNATURE-----\n";
        public const string BadFormattedModulus2 = "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHA256\n\nS/hBgmVXHlpzUxgzOlt4veE3v3BnpaV\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\n-----END PGP SIGNATURE-----\n";

        public const string WrongCRCModulus1 = "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHA256\n\nS/hBgmVXHlpzUxgzOlt4veE3v3BnpaVyRFUUDMmRgcF2yZU5rQcQYHDBGrnQAlGdcsGmZVcZC51JgJtEB6v5bBpxnnsjg8XibZm0GYXODhm7qki5wM5AEKoTKbZKaKuRD297pPTsVdqUdXFNdkDxk3Q3nv3N6ZEJccCS1IabllN+/adVTjUfCMA9pyJavOOj90fhcCQ2npInsxegvlGvREr1JpobdrtbXAOzLH+9ELxpW91ZFWbN0HHaE8+JV8TsZnhY+W0pqL+x18iVBwOCKjqiNVlXsJsd4PV0fyX3Fb/uRTnUuEYe/98xo+qqG/CrhIW7QgiuwemEN7PdHHARnQ==\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nwl4EARYIABAFAlwB1j0JEDUFhcTpUY8mAADqcAEA3ZzmaFqnbwCxfGqupfOL\nv8s+Z2PoHQ5KjSkXzMW1RZEA/R1s3YA4h/mChLxEFYEgNHHaRqLh3fmLXY8q\nkd76+7UO\n=hVVa\n-----END PGP SIGNATURE-----\n";
        public const string WrongCRCModulus2 = "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHA256\n\nS/hBgmVXHlpzUxgzOlt4veE3v3BnpaVyRFUUDMmRgcF2yZU5rQcQYHDBGrnQAlGdcsGmZVc\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nwl4EARYIABAFAlwB1j0JEDUFhcTpUY8mAADqcAEA3ZzmaFqnbwCxfGqupfOL\nv8s+Z2PoHQ5KjSkXzMW1RZEA/R1s3YA4h/mChLxEFYEgNHHaRqLh3fmLXY8q\nkd76+7U\n=hVVV\n-----END PGP SIGNATURE-----\n";

        public const string BadHeaderModulus1 = "-----BEGIN PGP SIGNED MESSAGE-----\nHashTag: SHA256\n\nS/hBgmVXHlpzUxgzOlt4veE3v3BnpaVyRFUUDMmRgcF2yZU5rQcQYHDBGrnQAlGdcsGmZVcZC51JgJtEB6v5bBpxnnsjg8XibZm0GYXODhm7qki5wM5AEKoTKbZKaKuRD297pPTsVdqUdXFNdkDxk3Q3nv3N6ZEJccCS1IabllN+/adVTjUfCMA9pyJavOOj90fhcCQ2npInsxegvlGvREr1JpobdrtbXAOzLH+9ELxpW91ZFWbN0HHaE8+JV8TsZnhY+W0pqL+x18iVBwOCKjqiNVlXsJsd4PV0fyX3Fb/uRTnUuEYe/98xo+qqG/CrhIW7QgiuwemEN7PdHHARnQ==\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nwl4EARYIABAFAlwB1j0JEDUFhcTpUY8mAADqcAEA3ZzmaFqnbwCxfGqupfOL\nv8s+Z2PoHQ5KjSkXzMW1RZEA/R1s3YA4h/mChLxEFYEgNHHaRqLh3fmLXY8q\nkd76+7UO\n=hVVV\n-----END PGP SIGNATURE-----\n";
        public const string BadHeaderModulus2 = "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHO256\n\nS/hBgmVXHlpzUxgzOlt4veE3v3BnpaVyRFUUDMmRgcF2yZU5rQcQYHDBGrnQAlGdcsGmZVcZC51JgJtEB6v5bBpxnnsjg8XibZm0GYXODhm7qki5wM5AEKoTKbZKaKuRD297pPTsVdqUdXFNdkDxk3Q3nv3N6ZEJccCS1IabllN+/adVTjUfCMA9pyJavOOj90fhcCQ2npInsxegvlGvREr1JpobdrtbXAOzLH+9ELxpW91ZFWbN0HHaE8+JV8TsZnhY+W0pqL+x18iVBwOCKjqiNVlXsJsd4PV0fyX3Fb/uRTnUuEYe/98xo+qqG/CrhIW7QgiuwemEN7PdHHARnQ==\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nwl4EARYIABAFAlwB1j0JEDUFhcTpUY8mAADqcAEA3ZzmaFqnbwCxfGqupfOL\nv8s+Z2PoHQ5KjSkXzMW1RZEA/R1s3YA4h/mChLxEFYEgNHHaRqLh3fmLXY8q\nkd76+7UO\n=hVVV\n-----END PGP SIGNATURE-----\n";

        public const string RightModulus = "S/hBgmVXHlpzUxgzOlt4veE3v3BnpaVyRFUUDMmRgcF2yZU5rQcQYHDBGrnQAlGdcsGmZVcZC51JgJtEB6v5bBpxnnsjg8XibZm0GYXODhm7qki5wM5AEKoTKbZKaKuRD297pPTsVdqUdXFNdkDxk3Q3nv3N6ZEJccCS1IabllN+/adVTjUfCMA9pyJavOOj90fhcCQ2npInsxegvlGvREr1JpobdrtbXAOzLH+9ELxpW91ZFWbN0HHaE8+JV8TsZnhY+W0pqL+x18iVBwOCKjqiNVlXsJsd4PV0fyX3Fb/uRTnUuEYe/98xo+qqG/CrhIW7QgiuwemEN7PdHHARnQ==";
        public const string RightModulus2 = "cxppFHJLKpd2WRExtw197XHmR+xd67d45DWyUx8iDAbnyPZolH2jJekfI9n19GVdRPQvwBECs4mLpp6AikuwuPJ/uwzWl3Rqh6ei6QmzC1iBLAQURso7Wxy8AyE+VXLbup119yyj1JaF4KxLXU6E9mzqevPdMqYD1XI19BGHAlZGCCfjTalTyQA4eh0pIcjQmeJ/8OjfUqx/enRz4ZvuQ8KDn9+iK8b/+OqH3pQr5s6YDlatQAIStdBPCZd/XbHNaFWrAZix21J/t99mPwKpGfVya1wA6j6LS2XQ/FQYuwQLNm/ZLlMYyNEcbL0CeebRvb8Cpv09u3fXpvs2YC3xjQ==";
        public const string RightModulus3 = "e6JT9qI2YWlCVAiP7dTEWzbRqynayWp47TZlh/gJ4dU9rRW81+vK0pxdJAdyUERLglfFUrevrYM9mcOXT1emr1wVuV7/S07G5G3hD/1WK5PyhiK2Wh+kct4TS5zX8aMb3hiaO60guFFIO8gCltqf9yGKiycQqDv90KryknnqdZrYL0zn35dj1oeLMjajuTpap7Jow8NWDxuXH+sARHpdUG/fPdTZ5dQEPaOBuTvz7krg6aFk+LYaOI80pW1280/N0205Asz+UYUW1TkyvJGy2MI/382pW4UFfOmahzCkxYDuEhkD8hu/84rR2IWoe5QVFFZqrWgEQImIbun/jlaB9g==";
    }
}
