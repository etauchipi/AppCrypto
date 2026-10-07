# AppCrypto

## Descripción
**AppCrypto** es una biblioteca de clases (`Class Library`) escrita en C# que proporciona funcionalidades para encriptar y desencriptar cadenas de texto utilizando el algoritmo AES (Advanced Encryption Standard). Además, ofrece soporte opcional para validar si las cadenas desencriptadas o a encriptar tienen un formato XML válido.

## Pila Tecnológica (Tech Stack)
* **Lenguaje:** C#
* **Framework:** .NET Framework 4.5
* **Dependencias Principales:**
  * `System.Security.Cryptography` (para el algoritmo de encriptación mediante `RijndaelManaged`)
  * `System.Xml` (para la validación opcional de cadenas en formato XML)

## Requisitos Previos
* Windows OS (recomendado) o entorno compatible con .NET Framework.
* [Visual Studio](https://visualstudio.microsoft.com/) 2012 o superior, o [.NET Framework 4.5 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net45).
* Herramientas de compilación como `msbuild` (incluido con Visual Studio) o `dotnet CLI` si se configura retargeting.

## Instalación y Configuración Local

1. **Clonar el repositorio:**
   ```bash
   git clone <url-del-repositorio>
   cd <carpeta-del-proyecto>
   ```

2. **Abrir el proyecto:**
   Abre el archivo de la solución `AppCrypto.sln` usando Visual Studio o tu IDE preferido.

3. **Compilar el proyecto:**
   Puedes compilar el proyecto desde Visual Studio (Ctrl + Shift + B) o usando la línea de comandos con MSBuild:
   ```bash
   msbuild AppCrypto.sln /p:Configuration=Release
   ```
   *(Nota: Asegúrate de tener MSBuild agregado a tus variables de entorno `PATH` o usa el "Developer Command Prompt" de Visual Studio).*

4. **Uso de la biblioteca:**
   El resultado de la compilación generará una DLL (`AppCrypto.dll`) en la carpeta `AppCrypto/bin/Debug` o `AppCrypto/bin/Release`. Puedes agregar esta DLL como referencia en otros proyectos .NET para utilizar sus funcionalidades.

## Estructura de Carpetas

```text
/
├── AppCrypto.sln           # Archivo principal de la solución para Visual Studio
└── AppCrypto/              # Directorio principal del proyecto de biblioteca
    ├── AppCrypto.csproj    # Archivo de configuración del proyecto C#
    ├── AppCrypto.cs        # Código fuente principal con la lógica de encriptación
    └── Properties/         # Configuración de los ensamblados e información del proyecto
```

## Guía Básica de Uso

A continuación se muestra un ejemplo básico de cómo utilizar la clase `CryptoAES` para encriptar y desencriptar texto en tu propia aplicación.

### Ejemplo de Encriptación

```csharp
using System;
using AppCrypto;

class Program
{
    static void Main()
    {
        // 1. Instanciar la clase AES
        AppCrypto.AppCrypto.CryptoAES crypto = new AppCrypto.AppCrypto.CryptoAES();

        // 2. Inicializar con la llave maestra (se usará como key e IV)
        // La llave debe tener una longitud de 16, 24 o 32 bytes para AES.
        string[] args = new string[] { "UnaLlaveSecreta1" };
        crypto.New(args);

        // 3. Preparar el mensaje de entrada
        AppCrypto.AppCrypto.MensajeIn mensajeIn = new AppCrypto.AppCrypto.MensajeIn
        {
            CadenaMsg = "Texto a encriptar",
            esXML = false // Cambiar a true si el contenido es XML y se desea validar
        };

        // 4. Ejecutar el proceso de encriptación
        AppCrypto.AppCrypto.MensajeOut mensajeOut = crypto.EncriptarMsg(mensajeIn, AppCrypto.AppCrypto.Proceso.Encriptar);

        if (!mensajeOut.esError)
        {
            Console.WriteLine("Texto encriptado (Base64): " + mensajeOut.CadenaMsg);
        }
        else
        {
            Console.WriteLine("Ocurrió un error en la encriptación o el XML no era válido.");
        }
    }
}
```

### Ejemplo de Desencriptación

```csharp
// (Asumiendo que 'crypto' ya fue inicializado con la misma llave y tienes 'mensajeOut')

// 1. Preparar el mensaje encriptado
AppCrypto.AppCrypto.MensajeIn mensajeInDesc = new AppCrypto.AppCrypto.MensajeIn
{
    CadenaMsg = mensajeOut.CadenaMsg, // Cadena en formato Base64 obtenida previamente
    esXML = false
};

// 2. Ejecutar el proceso de desencriptación
AppCrypto.AppCrypto.MensajeOut mensajeOutDesc = crypto.EncriptarMsg(mensajeInDesc, AppCrypto.AppCrypto.Proceso.Desencriptar);

if (!mensajeOutDesc.esError)
{
    Console.WriteLine("Texto desencriptado: " + mensajeOutDesc.CadenaMsg);
}
```