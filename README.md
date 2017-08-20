# fcfh
4chan file host

This application encodes information into images so they can be uploaded to 4chan or any other image hosting site.

## Modes of operation

### Encoding

Depending on the image format you might be able to encode information in multiple ways.
Encoding will always also store the original file name for later decoding.
In the case of pixel mode,
the application tries to generate images that are as close to a square as possible.

#### PNG

PNG can be used in two modes, pixel mode and header mode.
Header mode inserts a custom PNG header with your data in it,
this can be done multiple times to encode multiple files into a single image.
For header mode you need an existing PNG image.
Ideally this image contains instructions on how to decode it.
Pixel mode will use the image data to save your content.
Because PNG is a compressed image format you will not be able to see
the original binary content in a hex editor.
You need to convert the image to a BMP first or read the pixels individually.

#### BMP

Bitmap files are weird because the image data starts at the bottom left and goes up.
You can store the information as pixel data only.
You have the option to store it in a way that the raw binary data is still in order.

### Decode

Decoding detects the type of encoding automatically.
You only need to know if it was encrypted or not.

### Encryption

You can store content encrypted.
This uses industry standard AES encryption.
The key generation was made artificially more difficult to reduce the risks of brute-force attacks.

**DO NOT FORGET THE PASSWORD!**

There is no known backdoor or weakness in AES that would help you recover the content if you no longer have the password.

## Stability of images

You can only decode your information if the image was not altered.

### Pixel mode

The pixels must not be changed as every pixel is needed for decoding with very few exceptions.
This means you can only use lossless formats like bmp or png and are free to convert between them.
If you convert to a lossy format like JPEG, the information is lost.

### Header mode

Headers usually don't get converted to the new format if changing image formats
but they sometimes don't get erased when uploading the image to a 3rd party,
this makes them harder to detect, unless you store an image that looks way too small for its resolution.

# Usage Example

I made a simple [JavaScript example](https://cable.ayra.ch/imgplay/).
This Example implementation will only decode the first embedded data block found in PNG headers.
It does not supports pixel mode and will not check if the data is actually a supported audio format.
There are no plans for expanding the JS implementation.

# Standard

This chapter describes how the Data is Encoded.

All Integers are stored in network byte order.
According to [RFC 1700](https://tools.ietf.org/html/rfc1700) this is Big endian.

| Field      | Type  | Length (bytes) | Description         |
| ---------- | ----- | -------------- | ------------------- |
| ID         | Bytes | 6              | Always "BMPENC"     |
| NameLength | Int32 | 4              | Length of File Name |
| Name       | UTF-8 | NameLength     | File name           |
| DataLength | Int32 | 4              | Length of Data      |
| Data       | Bytes | DataLength     | Payload             |

## Detailed Field description

### ID

The ID field MUST be made up of the byte sequence `42 4D 50 45 4E 43`, which translates to ASCII `BMPENC`.
It is case sensitive.

### NameLength

This is the Length of the File Name. The Length MUST be Specified in Bytes and not in UTF-8 Chars.

### Name

This is the File Name. The File Name MUST NOT contain a Byte Order Mark.
The File Name MUST NOT be null Terminated.
An Encoder MUST NOT store the full or relative Path Name, only the File Name.
A Decoder MUST NOT interpret the Path Name.

### DataLength

This is the Length of the Payload in Bytes.
This Field is always required, even if there is no extra data after the payload.
The DataLength MUST NOT span over multiple Headers

### Data

This is the Data that has been encoded in the image.
The Encoder and Decoder MUST NOT transform the data in any way and always treat it as an 8-bit binary sequence.

# Multiple Files

Each Image can store one Pixel Mode File.
Some Images allow for multiple custom Headers.
In this case, each Header can contain a single File.

The format does not supports path names.
If an Encoder wants to store entire Directory Structures I recommend to store the Structure in an existing File container Format (zip, tar, 7z, etc) and then store the single container File in the Image.

# Encryption

Encryption is not part of the standard because there are enough encryption tools already. FCFH implements [ACRYPT](https://github.com/AyrA/crypt) as an example.
This is in no way an indication that an Encoder or Decoder should implement the same algorithm.
There are enough tools that properly encrypt and decrypt files

# TODO

Not necessarily in order

- [X] Implement Pixel mode Encode
- [X] Implement Pixel mode Decode
- [X] Implement Header mode Encode
- [X] Implement Header mode Decode
- [X] Make integers consistant (network byte order)
- [X] File name functions for Decode
- [X] Implement Encryption component
- [X] Implement Decryption component
- [X] Implement Command line switches
- [ ] GUI
- [X] Publish (some sort of) standard
